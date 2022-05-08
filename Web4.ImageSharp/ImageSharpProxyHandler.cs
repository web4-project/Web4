// <copyright file="ImageSharpProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.Processing;
using Web4.Core;

namespace Web4.ImageSharp
{
    /// <summary>
    /// Image Sharp Proxy Handler.
    /// </summary>
    public class ImageSharpProxyHandler : IImageProxyHandler
    {
        private HttpFileClient httpClient;
        private ImageTranscodeOptions defaultOptions;
        private string cachePath;
        private SixLabors.ImageSharp.Configuration imageConfig;

        public ImageSharpProxyHandler(HttpFileClient? httpClient = default, ImageTranscodeOptions? defaultOptions = null, string? defaultDownloadPath = default)
        {
            this.imageConfig = SixLabors.ImageSharp.Configuration.Default;
            this.imageConfig.MemoryAllocator = MemoryAllocator.Default;
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

        public SixLabors.ImageSharp.Configuration ImageConfig => this.imageConfig;

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

            using var contentStream = File.OpenRead(filepath);
            using var transcodeImage = await Image.LoadAsync(this.imageConfig, contentStream);
            using var fileStream = new FileStream(generatedFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if ((options.Width > 0 && options.Width <= transcodeImage.Width) || (options.Height > 0 && options.Height <= transcodeImage.Height))
            {
                transcodeImage.Mutate(x => x.Resize(options.Width, options.Height));
            }

            switch (options.Format)
            {
                case ImageFormat.Unknown:
                    break;
                case ImageFormat.bmp:
                    await transcodeImage.SaveAsBmpAsync(fileStream);
                    break;
                case ImageFormat.jpg:
                    await transcodeImage.SaveAsJpegAsync(fileStream);
                    break;
                case ImageFormat.gif:
                    await transcodeImage.SaveAsGifAsync(fileStream);
                    break;
                case ImageFormat.png:
                    await transcodeImage.SaveAsPngAsync(fileStream);
                    break;
                default:
                    break;
            }

            return new FileResult(generatedFilePath, md5);
        }
    }
}
