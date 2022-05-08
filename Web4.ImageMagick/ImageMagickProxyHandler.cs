﻿// <copyright file="ImageMagickProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using ImageMagick;
using Microsoft.AspNetCore.Http;
using Web4.Core;

namespace Web4.ImageMagick
{
    /// <summary>
    /// ImageMagic Proxy Handler.
    /// </summary>
    public class ImageMagickProxyHandler : IImageProxyHandler
    {
        private HttpFileClient httpClient;
        private ImageTranscodeOptions defaultOptions;
        private string cachePath;

        public ImageMagickProxyHandler(HttpFileClient? httpClient = default, ImageTranscodeOptions? defaultOptions = null, string? defaultDownloadPath = default)
        {
            var downloadPath = defaultDownloadPath ?? "Images";
            this.httpClient = httpClient ?? new HttpFileClient(defaultDownloadPath: downloadPath);
            this.defaultOptions = defaultOptions ?? new ImageTranscodeOptions();
            this.cachePath = Path.Combine(this.httpClient.DefaultDownloadPath, "Cache");
            Directory.CreateDirectory(this.cachePath);
        }

        /// <inheritdoc/>
        public string DefaultDownloadPath => this.httpClient.DefaultDownloadPath;

        /// <inheritdoc/>
        public string DefaultCachePath => this.cachePath;

        /// <inheritdoc/>
        public async Task InvokeImageProxy(HttpContext context)
        {
            try
            {
                var query = context.Request.Query;
                var path = Helpers.GetUrlFromPath(context.Request.Path);
                if (path is null)
                {
                    return;
                }

                var imageTranscodeOptions = ImageTranscodeOptions.FromQueryString(query);
                var resultLocation = await this.TranscodeImageAsync(path, imageTranscodeOptions);
                var bvtes = File.ReadAllBytes(resultLocation.FilePath);
                await context.Response.Body.WriteAsync(bvtes, 0, bvtes.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public Task<FileResult> TranscodeImageAsync(string filepath, ImageTranscodeOptions options, CancellationToken? cancellationToken = null)
            => this.TranscodeImageAsync(new Uri(filepath), options, cancellationToken);

        /// <inheritdoc/>
        public async Task<FileResult> TranscodeImageAsync(Uri uri, ImageTranscodeOptions options, CancellationToken? cancellationToken = null)
        {
            string filepath = string.Empty;

            // Generate the filename key based on the URI.
            string key = uri.ToString().GenerateKey() ?? string.Empty;
            string optionsKey = options.GenerateKey() ?? string.Empty;
            var generatedFileName = $"{key}-{optionsKey}.{options.Format}";

            // If file is remote and doesn't exist in the cache, download it.
            if (!uri.IsFile)
            {
                filepath = await this.httpClient.DownloadFile(uri, filename: key, useCache: options.UseCache);
            }
            else
            {
                filepath = uri.AbsolutePath;
            }

            if (!File.Exists(filepath))
            {
                throw new ArgumentNullException($"File does not exist: Filepath {filepath}, Uri: {uri}");
            }

            var md5 = Helpers.GenerateKey(filepath);
            var generatedFilePath = Path.Combine(this.cachePath, generatedFileName);

            using var image = new MagickImage(filepath);

            if ((options.Width > 0 && options.Width <= image.Width) || (options.Height > 0 && options.Height <= image.Height))
            {
                image.Resize(options.Width, options.Height);
            }

            switch (options.Format)
            {
                case ImageFormat.Unknown:
                    break;
                case ImageFormat.bmp:
                    image.Format = MagickFormat.Bmp;
                    break;
                case ImageFormat.jpg:
                    image.Format = MagickFormat.Jpg;
                    break;
                case ImageFormat.gif:
                    image.Format = MagickFormat.Gif;
                    break;
                case ImageFormat.png:
                    image.Format = MagickFormat.Png;
                    break;
                default:
                    break;
            }

            image.Write(generatedFilePath);

            return new FileResult(generatedFilePath, md5);
        }
    }
}
