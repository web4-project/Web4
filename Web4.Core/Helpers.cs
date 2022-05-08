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

        public static async Task<FilePathResult> GenerateFilePath(IHttpFileClient httpClient, Uri uri, object options, string extension, bool useCache, string cachePath, string? downloadPath = default)
        {
            string filepath = string.Empty;

            // Generate the filename key based on the URI.
            string key = uri.ToString().GenerateKey() ?? string.Empty;
            string optionsKey = options.GenerateKey() ?? string.Empty;
            var generatedFileName = $"{key}-{optionsKey}.{extension}";

            // If file is remote and doesn't exist in the cache, download it.
            if (!uri.IsFile)
            {
                filepath = await httpClient.DownloadFile(uri, filename: key, useCache: useCache, downloadPath: downloadPath);
            }
            else
            {
                filepath = uri.AbsolutePath;
            }

            if (!File.Exists(filepath))
            {
                throw new ArgumentNullException($"File does not exist: Filepath {filepath}, Uri: {uri}");
            }

            var md5 = Helpers.GenerateKey(filepath) ?? string.Empty;
            var generatedFilePath = Path.Combine(cachePath, generatedFileName);
            return new FilePathResult(filepath, generatedFilePath, generatedFileName, md5);
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

        public static string GenerateAudioExtension(this AudioCodec codec)
        {
            switch (codec)
            {
                case AudioCodec.Unknown:
                    return "unknown";
                case AudioCodec.pcm_u8:
                    return "pcm";
                case AudioCodec.real_144:
                    return "ra";
                case AudioCodec.adpcm_ms:
                    return "pcm";
                case AudioCodec.wmav1:
                    return "asf";
                default:
                    return "unknown";
            }
        }

        public static string GenerateVideoExtension(this VideoFormat codec)
        {
            switch (codec)
            {
                case VideoFormat.None:
                    return "unknown";
                case VideoFormat.rm:
                    return "rm";
                case VideoFormat.asf:
                    return "wmv";
                case VideoFormat.mov:
                    return "mov";
                case VideoFormat.avi:
                    return "avi";
                default:
                    return "unknown";
            }
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
