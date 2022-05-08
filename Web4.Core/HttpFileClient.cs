// <copyright file="HttpFileClient.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Web4.Core
{
    /// <summary>
    /// Http File Client.
    /// </summary>
    public class HttpFileClient : IHttpFileClient
    {
        private HttpClient httpClient;
        private ILogger<HttpFileClient>? logger;
        private string defaultDownloadPath;

        public HttpFileClient(HttpClient? client = default, ILogger<HttpFileClient>? logger = default, string? defaultDownloadPath = default)
        {
            this.httpClient = client ?? new HttpClient();
            this.logger = logger ?? default;
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty);
            this.defaultDownloadPath = Path.Combine(path, defaultDownloadPath ?? "Downloads");
        }

        /// <inheritdoc/>
        public string DefaultDownloadPath => this.defaultDownloadPath;

        /// <inheritdoc/>
        public async Task<string> DownloadFile(Uri uri, string? downloadPath = default, string? filename = default)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));
            this.logger?.LogInformation($"Downloading File from {uri}.");

            downloadPath = downloadPath ?? this.defaultDownloadPath;
            filename = filename ?? Path.GetTempFileName();
            var fileInfo = new FileInfo(Path.Combine(downloadPath, filename));
            var response = await this.httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            await using var ms = await response.Content.ReadAsStreamAsync();
            await using var fs = File.Create(fileInfo.FullName);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
            this.logger?.LogInformation($"File saved as [{fileInfo.Name}].");

            return fileInfo.FullName;
        }

        /// <inheritdoc/>
        public Task<string> UploadFile(string filePath, MultipartFormDataContent? dataForm = null)
        {
            throw new NotImplementedException();
        }
    }
}
