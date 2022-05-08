// <copyright file="FFMpegVideoProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Web4.Core;
using Xabe.FFmpeg;
using System.Drawing;

namespace Web4.FFMpeg
{
    /// <summary>
    /// FFMpeg Video Proxy Handler.
    /// </summary>
    public class FFMpegVideoProxyHandler : IVideoProxyHandler
    {
        private HttpFileClient httpClient;
        private VideoTranscodeOptions defaultOptions;
        private string cachePath;

        public FFMpegVideoProxyHandler(HttpFileClient? httpClient = default, VideoTranscodeOptions? defaultOptions = null, string? defaultDownloadPath = default)
        {
            var downloadPath = defaultDownloadPath ?? "Video";
            this.httpClient = httpClient ?? new HttpFileClient(defaultDownloadPath: downloadPath);
            this.defaultOptions = defaultOptions ?? new VideoTranscodeOptions();
            this.cachePath = Path.Combine(this.httpClient.DefaultDownloadPath, "Cache");
            Directory.CreateDirectory(this.cachePath);
        }

        /// <inheritdoc/>
        public string DefaultDownloadPath => this.httpClient.DefaultDownloadPath;

        /// <inheritdoc/>
        public string DefaultCachePath => this.cachePath;

        /// <inheritdoc/>
        public async Task InvokeVideoProxy(HttpContext context)
        {
            try
            {
                var query = context.Request.Query;
                var path = Helpers.GetUrlFromPath(context.Request.Path);
                if (path is null)
                {
                    return;
                }

                var transcodeOptions = VideoTranscodeOptions.FromQueryString(query);
                var resultLocation = await this.TranscodeVideoAsync(path, transcodeOptions);
                var filename = Path.GetFileNameWithoutExtension(resultLocation.FilePath);

                context.AddContentTypeHeaders(filename, transcodeOptions);

                using var fileStream = File.OpenRead(resultLocation.FilePath);
                await fileStream.CopyToAsync(context.Response.Body);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public Task<FileResult> TranscodeVideoAsync(string filepath, VideoTranscodeOptions options, CancellationToken? cancellationToken = null)
            => this.TranscodeVideoAsync(new Uri(filepath), options, cancellationToken);

        /// <inheritdoc/>
        public async Task<FileResult> TranscodeVideoAsync(Uri uri, VideoTranscodeOptions options, CancellationToken? cancellationToken = null)
        {
            var result = await Helpers.GenerateFilePath(this.httpClient, uri, options, options.Format.GenerateVideoExtension(), options.UseCache, this.cachePath, null);

            var fileResult = await this.StartVideoTranscode(result.FilePath, result.GeneratedFilePath, options, cancellationToken);
            var md5 = Helpers.GenerateKey(fileResult) ?? string.Empty;
            return new FileResult(result.GeneratedFilePath, md5);
        }

        private async Task<string> StartVideoTranscode(string input, string output, VideoTranscodeOptions options, CancellationToken? cancellationToken = null)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (options.VCodec == Web4.Core.VideoCodec.Unknown)
            {
                throw new ArgumentException(nameof(options.ACodec));
            }

            if (options.ACodec == Web4.Core.AudioCodec.Unknown)
            {
                throw new ArgumentException(nameof(options.ACodec));
            }

            var conversion = FFmpeg.Conversions.New();
            conversion.AddParameter($"-i {input}");
            conversion.AddParameter($"-f {options.Format}");

            conversion.AddParameter($"-vcodec {options.VCodec}");

            var scaleFormat = await this.GenerateScale(input, options);
            if (!string.IsNullOrEmpty(scaleFormat))
            {
                conversion.AddParameter($"-vf {scaleFormat}");
            }

            if (options.VBitrate > 0)
            {
                conversion.AddParameter($"-b:v {options.VBitrate}");
            }

            conversion.AddParameter($"-acodec {options.ACodec}");

            if (options.ABitrate > 0)
            {
                conversion.AddParameter($"-b:a {options.ABitrate}");
            }

            if (options.SampleRate != SampleRate.Unknown)
            {
                var sampleRate = options.SampleRate.ToString().Replace("_", string.Empty);
                conversion.AddParameter($"-ar {sampleRate}");
            }

            conversion.SetOutput(output);

            var result = await conversion.Start();

            if (File.Exists(output))
            {
                return output;
            }

            throw new Exception("Failed to transcode video");
        }

        private async Task<string> GenerateScale(string url, VideoTranscodeOptions options)
        {
            if (options.Format == VideoFormat.None)
            {
                return string.Empty;
            }

            var mediaInfo = await FFmpeg.GetMediaInfo(url);
            if (mediaInfo is null)
            {
                return string.Empty;
            }

            var video = mediaInfo.VideoStreams.FirstOrDefault();
            if (video is null)
            {
                return string.Empty;
            }

            int divisibleBy = 1;

            switch (options.Format)
            {
                case VideoFormat.rm:
                    divisibleBy = 16;
                    break;
            }

            var resizeValue = 1;
            if (options.ScaleDownBy > 0)
            {
                resizeValue = options.ScaleDownBy;
            }

            int orgWidth = video.Width;
            int orgHeight = video.Height;

            SizeF size = default;

            switch (options.VideoQuality)
            {
                case VideoQuality.Unknown:
                    break;
                case VideoQuality._96p:
                    size = this.ConstrainConcise(video.Width, video.Height, 128, 96);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._120p:
                    size = this.ConstrainConcise(video.Width, video.Height, 160, 120);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._144p:
                    size = this.ConstrainConcise(video.Width, video.Height, 256, 144);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._180p:
                    size = this.ConstrainConcise(video.Width, video.Height, 320, 180);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._240p:
                    size = this.ConstrainConcise(video.Width, video.Height, 426, 240);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._272p:
                    size = this.ConstrainConcise(video.Width, video.Height, 480, 272);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._288p:
                    size = this.ConstrainConcise(video.Width, video.Height, 384, 288);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._360p:
                    size = this.ConstrainConcise(video.Width, video.Height, 480, 360);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                case VideoQuality._480p:
                    size = this.ConstrainConcise(video.Width, video.Height, 640, 480);
                    orgWidth = (int)size.Width;
                    orgHeight = (int)size.Height;
                    break;
                default:
                    break;
            }

            var width = this.ProperScaleValue(orgWidth, resizeValue, divisibleBy);
            var height = this.ProperScaleValue(orgHeight, resizeValue, divisibleBy);
            return $"scale={width}:{height}";
        }

        private SizeF ConstrainConcise(int width, int height, int maxWidth, int maxHeight)
        {
            // Downscale by the smallest ratio (never upscale)
            var scale = Math.Min(1, Math.Min(maxWidth / (float)width, maxHeight / (float)height));
            return new SizeF(scale * width, scale * height);
        }

        private int ProperScaleValue(int orgValue, int scaleBy, int divisibleBy)
        {
            var newValue = scaleBy < 0 ? orgValue / (scaleBy * -1) : orgValue * scaleBy;
            while (newValue > 0 && newValue % divisibleBy != 0)
            {
                newValue -= 1;
            }

            return newValue;
        }
    }
}
