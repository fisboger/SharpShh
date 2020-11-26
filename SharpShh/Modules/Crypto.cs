using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharpShh.Modules
{

    public static class Crypto
    {
        public static byte[] Encrypt(byte[] input, out byte[] key, out byte[] iv)
        {
            using(var aes = new AesManaged())
            {
                key = aes.Key;
                iv = aes.IV;

                return Encrypt(input, key, iv);
            }
        }
        public static byte[] Encrypt(byte[] input, byte[] key, byte[] iv)
        {
            using(var aes = new AesManaged())
            {
                aes.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                using(var memoryStream = new MemoryStream())
                {
                    using(var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(input, 0, input.Length);
                        cryptoStream.FlushFinalBlock();

                        return memoryStream.ToArray();
                    }
                }

            }
        }

        public static byte[] Decrypt(byte[] input, byte[] key, byte[] iv)
        {
            using (var aes = new AesManaged())
            {
                aes.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = aes.CreateDecryptor(key, iv);

                using (var memoryStream = new MemoryStream(input))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read))
                    {
                        cryptoStream.Read(input, 0, input.Length);

                        return memoryStream.ToArray();
                    }
                }

            }
        }

        // https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Base64Encode(plainTextBytes);
        }

        public static string Base64Encode(byte[] input)
        {
            return System.Convert.ToBase64String(input);
        }

        // https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
