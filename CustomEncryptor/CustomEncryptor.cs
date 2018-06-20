using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static System.Console;

namespace CustomEncryptor
{
    public class CustomEncryptor
    {
        public static void Main(string[] args)
        {
            Dictionary dictionary = new Dictionary();
            dictionary.GenerateKeys(8);

            string plaintext = string.Empty;
            while(string.IsNullOrEmpty(plaintext))
            {
                Write("Please enter the text you want to encrypt: ");
                plaintext = ReadLine();
            }

            WriteLine("Please copy-paste your selected key from the following list to encrypt your plain-text.");
            WriteLine();

            foreach (string key in dictionary.Keys)
                WriteLine(key);
            WriteLine();

            string selectedKey = string.Empty;
            while (string.IsNullOrEmpty(selectedKey) || !dictionary.Keys.Contains(selectedKey))
            {
                WriteLine("Please copy-paste your selected key from the following list to encrypt your plain-text.");
                selectedKey = ReadLine();
            }
            WriteLine();

            string cipher = Encrypt(plaintext, selectedKey);
            WriteLine($"Encrypted text: {cipher}");
            WriteLine();

            string plaintextResult = Decrypt(cipher, selectedKey);
            WriteLine($"Decrypted text: {plaintextResult}");
            WriteLine();

            Write("Press any key to exit...");
            ReadKey();
        }

        public static string Encrypt(string plaintext, string key)
        {
            try
            {
                string reversedKey = Reverse(key);                                                          // Reverse the selected key.

                byte[] cipher = new byte[plaintext.Length];
                byte[] reverseCipher = new byte[plaintext.Length];

                for (int i = 0; i < plaintext.Length; i++)
                    reverseCipher[i] = Convert.ToByte(plaintext[i] ^ reversedKey[i % reversedKey.Length]);  // Generate a reverse-cipher by XORing each reversed key character with the plaintext characters.

                for (int i = 0; i < plaintext.Length; i++)
                    cipher[i] = Convert.ToByte(reverseCipher[i] ^ key[i % key.Length]);                     // Generate a final-cipher by XORing each original key character with the reverse-cipher characters.

                return Encoding.ASCII.GetString(cipher);                                                    // Convert byte[] to string for printing on the console.
            }
            catch (Exception)
            { return string.Empty; }
        }

        public static string Decrypt(string cipher, string key)
        {
            try
            {
                string reversedKey = Reverse(key);                                                              // Reverse the selected key.

                byte[] plaintext = new byte[cipher.Length];
                byte[] reversedPlaintext = new byte[cipher.Length];

                for (int i = 0; i < cipher.Length; i++)
                    reversedPlaintext[i] = Convert.ToByte(cipher[i] ^ key[i % key.Length]);                      // Obtain the reverse-plaintext by XORing each key character with the final-cipher characters.

                for (int i = 0; i < cipher.Length; i++)
                    plaintext[i] = Convert.ToByte(reversedPlaintext[i] ^ reversedKey[i % reversedKey.Length]);   // Obtain the final-plaintext by XORing each reversed key character with the reverse-plaintext characters. 

                return Encoding.ASCII.GetString(plaintext);                                                      // Convert byte[] to string for printing on the console.
            }
            catch (Exception)
            { return string.Empty; }
        }

        /// <summary>
        /// Reverse the supplied text.
        /// </summary>
        /// <param name="text">Text to be reversed.</param>
        /// <returns>Reversed text.</returns>
        public static string Reverse(string text)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }
    }

    public class Dictionary
    {
        public List<string> Keys { get; set; }

        public Dictionary()
        { Keys = new List<string>(); }

        /// <summary>
        /// Custom string-key generator.
        /// Uses a class of lower-case, upper-case and numeric characters as a pool to generate keys from.
        /// </summary>
        /// <param name="count">Number of unique keys to generate.</param>
        public void GenerateKeys(int count)
        {
            try
            {
                char[] chars = new char[62];
                string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                chars = a.ToCharArray();
                int size = 32;

                for (int i = 0; i < count; i++)
                {
                    byte[] data = new byte[1];
                    RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                    data = new byte[size];
                    crypto.GetNonZeroBytes(data);
                    StringBuilder result = new StringBuilder(size);
                    foreach (byte b in data) result.Append(chars[b % (chars.Length - 1)]);
                    Keys.Add(result.ToString());
                }
            }
            catch (Exception)
            { }
        }
    }
}
