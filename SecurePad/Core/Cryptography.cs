using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurePad.Core
{

    public static class Cryptography
    {

        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private static readonly Random Randomizer = new();

        public static string EncryptData(string data, string key)
        {
            var array = Encoding.UTF8.GetBytes(data);
            var provider = new AesCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var encryptor = provider.CreateEncryptor();
            var result = encryptor.TransformFinalBlock(array, 0, array.Length);
            provider.Clear();
            return Convert.ToBase64String(result, 0, result.Length);
        }

        public static bool DecryptData(string data, string key, out string output)
        {
            try
            {
                var array = Convert.FromBase64String(data);
                var provider = new AesCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                var decryptor = provider.CreateDecryptor();
                var resultArray = decryptor.TransformFinalBlock(array, 0, array.Length);
                provider.Clear();
                output = Encoding.UTF8.GetString(resultArray);
                return true;
            }
            catch
            {
                output = null!;
                return false;
            }
        }

        public static string FixPasswordLength(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length > 16)
                throw new ArgumentOutOfRangeException(nameof(password));
            if (password.Length == 16)
                return password;
            var charsNeeded = 16 - password.Length;
            for (var index = 0; index < charsNeeded; index++) password += "X";
            return password;
        }

        public static string GenerateRandomKey()
        {
            return $"{GenerateRandomString(4)}-{GenerateRandomString(4)}-{GenerateRandomString(6)}";
        }

        private static string GenerateRandomString(int length)
        {
            return new(Enumerable.Repeat(Characters, length).Select(index => index[Randomizer.Next(index.Length)]).ToArray());
        }

    }

}