// <copyright file="HttpContextExtensions.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Web4.Core
{
    /// <summary>
    /// HttpContext Extension Methods.
    /// </summary>
    public static class HttpContextExtensions
    {
        public static async Task WriteContentsWithEncodingAsync(this HttpContext context, string content)
        {
            var culture = GetCulutreInfoViaHeaders(context);
            await context.Response.WriteAsync(content, System.Text.Encoding.GetEncoding(culture.TextInfo.OEMCodePage));
        }

        public static CultureInfo GetCulutreInfoViaHeaders(this HttpContext context)
        {
            StringValues ua = string.Empty;
            StringValues lang = string.Empty;

            context.Request.Headers.TryGetValue("User-Agent", out ua);
            context.Request.Headers.TryGetValue("Accept-Language", out lang);
            IOrderedEnumerable<StringWithQualityHeaderValue> languages = lang.ToString().Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1));

            return GetCultureInfoViaAcceptLanguage(lang);
        }

        public static CultureInfo GetCultureInfoViaAcceptLanguage(string acceptLanguage)
        {
            if (string.IsNullOrEmpty(acceptLanguage))
            {
                return CultureInfo.InvariantCulture;
            }

            var lang = GetSupportedLanguages(acceptLanguage).FirstOrDefault();
            if (lang == null)
            {
                return CultureInfo.InvariantCulture;
            }

            var culture = CultureInfo.GetCultureInfo(lang.Value);
            return culture;
        }

        public static IOrderedEnumerable<StringWithQualityHeaderValue> GetSupportedLanguages(string acceptLanguage)
        {
            return acceptLanguage.ToString().Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1));
        }

        public static void AddContentTypeHeaders(this HttpContext context, string filename, VideoTranscodeOptions options)
        {
            var fileExtension = string.Empty;
            switch (options.Format)
            {
                case VideoFormat.rm:
                    context.Response.ContentType = "application/octet-stream";
                    fileExtension = ".rm";
                    break;
                case VideoFormat.asf:
                    context.Response.ContentType = "video/x-ms-asf";
                    fileExtension = ".wmv";
                    break;
                case VideoFormat.mov:
                    context.Response.ContentType = "video/quicktime";
                    fileExtension = ".mov";
                    break;
                case VideoFormat.avi:
                    context.Response.ContentType = "application/octet-stream";
                    fileExtension = ".avi";
                    break;
                default:
                    context.Response.ContentType = "application/octet-stream";
                    break;
            }

            context.Response.Headers.Add("Content-Disposition", $"attachment; filename={filename}{fileExtension}");
        }

        public static void AddContentTypeHeaders(this HttpContext context, string filename, AudioTranscodeOptions options)
        {
            var fileExtension = string.Empty;
            switch (options.ACodec)
            {
                case AudioCodec.pcm_u8:
                    context.Response.ContentType = "application/octet-stream";
                    fileExtension = ".pcm";
                    break;
                case AudioCodec.real_144:
                    context.Response.ContentType = "application/octet-stream";
                    fileExtension = ".ra";
                    break;
                case AudioCodec.adpcm_ms:
                    context.Response.ContentType = "application/octet-stream";
                    fileExtension = ".pcm";
                    break;
                case AudioCodec.wmav1:
                    context.Response.ContentType = "application/octet-stream";
                    fileExtension = ".asf";
                    break;
                default:
                    context.Response.ContentType = "application/octet-stream";
                    break;
            }

            context.Response.Headers.Add("Content-Disposition", $"attachment; filename={filename}{fileExtension}");
        }
    }
}
