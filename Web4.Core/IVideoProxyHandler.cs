// <copyright file="IVideoProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Web4.Core
{
    /// <summary>
    /// Video Proxy Handler.
    /// </summary>
    public interface IVideoProxyHandler
    {
        /// <summary>
        /// Gets the default download path.
        /// </summary>
        string DefaultDownloadPath { get; }

        /// <summary>
        /// Gets the default cache path.
        /// </summary>
        string DefaultCachePath { get; }

        /// <summary>
        /// Invoke Video Proxy for a given video file.
        /// </summary>
        /// <param name="context"><see cref="HttpContent"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task InvokeVideoProxy(HttpContext context);

        /// <summary>
        /// Transcodes an video file.
        /// </summary>
        /// <param name="filepath">Filepath to the video file.</param>
        /// <param name="options"><see cref="VideoTranscodeOptions"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>String Path location to the video file.</returns>
        Task<FileResult> TranscodeVideoAsync(string filepath, VideoTranscodeOptions options, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Transcodes an video file.
        /// </summary>
        /// <param name="uri">URI to the video file.</param>
        /// <param name="options"><see cref="VideoTranscodeOptions"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>String Path location to the video file.</returns>
        Task<FileResult> TranscodeVideoAsync(Uri uri, VideoTranscodeOptions options, CancellationToken? cancellationToken = default);
    }
}
