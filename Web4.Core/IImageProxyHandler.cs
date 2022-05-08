// <copyright file="IImageProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Web4.Core
{
    /// <summary>
    /// Image Proxy Handler.
    /// </summary>
    public interface IImageProxyHandler
    {
        /// <summary>
        /// Invoke Image Proxy for a given image.
        /// </summary>
        /// <param name="context"><see cref="HttpContent"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task InvokeImageProxy(HttpContext context);

        /// <summary>
        /// Transcodes an image.
        /// </summary>
        /// <param name="filepath">Filepath to the image.</param>
        /// <param name="options"><see cref="ImageTranscodeOptions"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>String Path location to the image.</returns>
        Task<string> TranscodeImageAsync(string filepath, ImageTranscodeOptions options, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Transcodes an image.
        /// </summary>
        /// <param name="uri">URI to the image.</param>
        /// <param name="options"><see cref="ImageTranscodeOptions"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>String Path location to the image.</returns>
        Task<string> TranscodeImageAsync(Uri uri, ImageTranscodeOptions options, CancellationToken? cancellationToken = default);

        string DefaultDownloadPath { get; }

        string DefaultCachePath { get; }
    }
}
