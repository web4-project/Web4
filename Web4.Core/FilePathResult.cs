// <copyright file="FilePathResult.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    public class FilePathResult
    {
        public FilePathResult(string filepath, string generatedFilePath, string generatedFilename, string md5)
        {
            this.FilePath = filepath;
            this.GeneratedFilePath = generatedFilePath;
            this.GeneratedFileName = generatedFilename;
            this.Md5 = md5;
        }

        public string FilePath { get; }

        public string GeneratedFilePath { get; }

        public string GeneratedFileName { get; }

        public string Md5 { get; }
    }
}
