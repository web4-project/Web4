// <copyright file="FFMpegAudioProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Web4.Core;
using Xabe.FFmpeg;

namespace Web4.FFMpeg
{
    /// <summary>
    /// FFMpeg Audio Proxy Handler.
    /// </summary>
    public class FFMpegAudioProxyHandler : IAudioProxyHandler
    {
        private HttpFileClient httpClient;
        private AudioTranscodeOptions defaultOptions;
        private string cachePath;

        public FFMpegAudioProxyHandler(HttpFileClient? httpClient = default, AudioTranscodeOptions? defaultOptions = null, string? defaultDownloadPath = default)
        {
            var downloadPath = defaultDownloadPath ?? "Audio";
            this.httpClient = httpClient ?? new HttpFileClient(defaultDownloadPath: downloadPath);
            this.defaultOptions = defaultOptions ?? new AudioTranscodeOptions();
            this.cachePath = Path.Combine(this.httpClient.DefaultDownloadPath, "Cache");
            Directory.CreateDirectory(this.cachePath);
        }

        /// <inheritdoc/>
        public string DefaultDownloadPath => this.httpClient.DefaultDownloadPath;

        /// <inheritdoc/>
        public string DefaultCachePath => this.cachePath;

        /// <inheritdoc/>
        public async Task InvokeAudioProxy(HttpContext context)
        {
            try
            {
                var query = context.Request.Query;
                var path = Helpers.GetUrlFromPath(context.Request.Path);
                if (path is null)
                {
                    return;
                }

                var transcodeOptions = AudioTranscodeOptions.FromQueryString(query);
                var resultLocation = await this.TranscodeAudioAsync(path, transcodeOptions);
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
        public Task<FileResult> TranscodeAudioAsync(string filepath, AudioTranscodeOptions options, CancellationToken? cancellationToken = null)
            => this.TranscodeAudioAsync(new Uri(filepath), options, cancellationToken);

        /// <inheritdoc/>
        public async Task<FileResult> TranscodeAudioAsync(Uri uri, AudioTranscodeOptions options, CancellationToken? cancellationToken = null)
        {
            var result = await Helpers.GenerateFilePath(this.httpClient, uri, options, options.ACodec.GenerateAudioExtension(), options.UseCache, this.cachePath, null);

            var fileResult = await this.StartAudioTranscode(result.FilePath, result.GeneratedFilePath, options, cancellationToken);
            var md5 = Helpers.GenerateKey(fileResult) ?? string.Empty;
            return new FileResult(result.GeneratedFilePath, md5);
        }

        private async Task<string> StartAudioTranscode(string input, string output, AudioTranscodeOptions options, CancellationToken? cancellationToken = null)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            if (options.ACodec == Web4.Core.AudioCodec.Unknown)
            {
                throw new ArgumentException(nameof(options.ACodec));
            }

            var conversion = FFmpeg.Conversions.New();
            conversion.AddParameter($"-i {input}");

            switch (options.ACodec)
            {
                case Web4.Core.AudioCodec.pcm_u8:
                    conversion.AddParameter($"-f f32be");
                    break;
                case Web4.Core.AudioCodec.real_144:
                    conversion.AddParameter($"-f rm");
                    break;
                case Web4.Core.AudioCodec.adpcm_ms:
                    conversion.AddParameter($"-f f32be");
                    break;
                case Web4.Core.AudioCodec.wmav1:
                    conversion.AddParameter($"-f asf");
                    break;
                default:
                    break;
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

            throw new Exception("Failed to transcode audio");
        }
    }
}
