using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EFDataService.Helper
{
    public static class SecurityHandler
    {
        public static string DefaultKey = "b14ca8oj8a4e41@3bbce2%a2315a1916";
        public static string EncryptString(string key, string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter((Stream)cryptoStream))
            {
                streamWriter.Write(plainText);
            }

            var array = memoryStream.ToArray();

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string encrptedText)
        {
            var buffer = Convert.FromBase64String(encrptedText);
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
