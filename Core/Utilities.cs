using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace SecurePad.Core
{

    internal static class Utilities
    {

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetConnectedState(out int flags, int reserved);

        public static bool IsUserOnline()
        {
            return InternetGetConnectedState(out _, 0);
        }

        public static bool IsUpdateAvailable()
        {
            var client = new WebClient();
            var data =
                client.DownloadString("https://raw.githubusercontent.com/dentolos19/SecurePad/master/VERSION");
            client.Dispose();
            return Version.Parse(data) < Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static string ToHexString(string data)
        {
            var builder = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(data);
            foreach (var item in bytes)
                builder.Append(item.ToString("X2"));
            return builder.ToString();
        }

        public static string FromHexString(string data)
        {
            var bytes = new byte[data.Length / 2];
            for (var index = 0; index < bytes.Length; index++)
                bytes[index] = Convert.ToByte(data.Substring(index * 2, 2), 16);
            return Encoding.Unicode.GetString(bytes);
        }

        public static string GetUniqueCode(string custom = null)
        {
            var raw = $"{Environment.MachineName}-{Environment.UserName}";
            if (!string.IsNullOrEmpty(custom))
                raw = custom;
            return ToHexString(raw);
        }

        public static void Restart(string args = null)
        {
            if (string.IsNullOrEmpty(args))
                args = string.Empty;
            var task = new Process
            {
                StartInfo =
                {
                    FileName = Assembly.GetExecutingAssembly().Location,
                    Arguments = args
                }
            };
            task.Start();
            Application.Current.Shutdown();
        }

        public static string Encrypt(string data, string key, bool hashing = true)
        {
            byte[] bytes;
            var buffer = Encoding.UTF8.GetBytes(data);
            if (hashing)
            {
                var md5 = new MD5CryptoServiceProvider();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                md5.Clear();
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(key);
            }
            var provider = new TripleDESCryptoServiceProvider
            {
                Key = bytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var transform = provider.CreateEncryptor();
            var result = transform.TransformFinalBlock(buffer, 0, buffer.Length);
            provider.Clear();
            return Convert.ToBase64String(result, 0, result.Length);
        }

        public static string Decrypt(string data, string key, bool hashing = true)
        {
            byte[] bytes;
            var buffer = Convert.FromBase64String(data);
            if (hashing)
            {
                var md5 = new MD5CryptoServiceProvider();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                md5.Clear();
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(key);
            }
            var provider = new TripleDESCryptoServiceProvider
            {
                Key = bytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var transform = provider.CreateDecryptor();
            var result = transform.TransformFinalBlock(buffer, 0, buffer.Length);
            provider.Clear();
            return Encoding.UTF8.GetString(result);
        }

    }

}