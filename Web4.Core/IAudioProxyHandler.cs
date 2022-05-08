// <copyright file="IAudioProxyHandler.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Web4.Core
{
    /// <summary>
    /// Audio Proxy Handler.
    /// </summary>
    public interface IAudioProxyHandler
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
        /// Invoke Audio Proxy for a given audio file.
        /// </summary>
        /// <param name="context"><see cref="HttpContent"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task InvokeAudioProxy(HttpContext context);

        /// <summary>
        /// Transcodes an audio file.
        /// </summary>
        /// <param name="filepath">Filepath to the audio file.</param>
        /// <param name="options"><see cref="AudioTranscodeOptions"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>String Path location to the audio file.</returns>
        Task<FileResult> TranscodeAudioAsync(string filepath, AudioTranscodeOptions options, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Transcodes an audio file.
        /// </summary>
        /// <param name="uri">URI to the audio file.</param>
        /// <param name="options"><see cref="AudioTranscodeOptions"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>String Path location to the audio file.</returns>
        Task<FileResult> TranscodeAudioAsync(Uri uri, AudioTranscodeOptions options, CancellationToken? cancellationToken = default);
    }
}
