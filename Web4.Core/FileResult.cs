// <copyright file="FileResult.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    public class FileResult
    {
        public FileResult(string? filePath, string? md5)        {
            this.FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            this.Md5 = md5 ?? throw new ArgumentNullException(nameof(md5));
        }

        public int Id { get; }

        public string FilePath { get; }

        public string Md5 { get; }
    }
}
