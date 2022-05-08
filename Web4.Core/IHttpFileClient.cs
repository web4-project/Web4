using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web4.Core
{
    public interface IHttpFileClient
    {
        Task<string> UploadFile(string filePath, MultipartFormDataContent? dataForm = default);

        Task<string> DownloadFile(Uri uri, string? downloadPath, string? filename, bool useCache = true);

        string DefaultDownloadPath { get; }
    }
}
