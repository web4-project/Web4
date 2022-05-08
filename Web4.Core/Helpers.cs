// <copyright file="Helpers.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Web4.Core
{
    /// <summary>
    /// Common Helpers.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Gets a URL from an encoded URL path.
        /// This is commonly used for the proxy commands, where we can have a full URL as a path.
        /// We need to find those paths and translate them back to proper URLs.
        /// </summary>
        /// <param name="path">URL Path.</param>
        /// <returns>HTML Path if valid, null if not.</returns>
        public static Uri? GetUrlFromPath(string path)
        {
            try
            {
                var substringIndexPath = path.IndexOf("http");
                if (substringIndexPath < 0)
                {
                    return null;
                }

                var substringPath = path.Substring(substringIndexPath);
                if (!substringPath.Contains("https://"))
                {
                    substringPath = substringPath.Replace("https:/", "https://");
                }

                if (!substringPath.Contains("http://"))
                {
                    substringPath = substringPath.Replace("http:/", "http://");
                }

                return new Uri(substringPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        public static string? GenerateKey(string filename)
        {
            using var md5Instance = MD5.Create();
            using var stream = File.OpenRead(filename);
            var hashResult = md5Instance.ComputeHash(stream);
            return BitConverter.ToString(hashResult).Replace("-", string.Empty).ToLowerInvariant();
        }

        public static string? GenerateKey(this object sourceObject)
        {
            ArgumentNullException.ThrowIfNull(sourceObject, "source");
            var hash = ComputeHash(ObjectToByteArray(sourceObject));
            if (hash is null)
            {
                return null;
            }

            return hash.Replace("-", string.Empty).ToLowerInvariant();
        }

        private static string? ComputeHash(byte[]? objectAsBytes)
        {
            if (objectAsBytes == null)
            {
                return null;
            }

            MD5 md5 = MD5.Create();
            try
            {
                byte[] result = md5.ComputeHash(objectAsBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("Hash has not been generated.");
                return null;
            }
        }

        /// <summary>
        /// Convert an object to a Byte Array.
        /// </summary>
        public static byte[]? ObjectToByteArray(object objData)
        {
            if (objData == null)
            {
                return default;
            }

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(objData, GetJsonSerializerOptions()));
        }

        /// <summary>
        /// Convert a byte array to an Object of T.
        /// </summary>
        public static T? ByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null || !byteArray.Any())
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(byteArray, GetJsonSerializerOptions());
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
