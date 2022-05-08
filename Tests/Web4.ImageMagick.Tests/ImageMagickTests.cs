// <copyright file="ImageMagickTests.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Reflection;

namespace Web4.ImageMagick.Tests;

/// <summary>
/// Image Magick Tests.
/// </summary>
[TestClass]
public class ImageMagickTests
{
    private ImageMagickProxyHandler proxyHandler;
    private string baseDirectory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageMagickTests"/> class.
    /// </summary>
    public ImageMagickTests()
    {
        var baseDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ImageMagickProxyHandler))?.Location) ?? string.Empty;
        this.baseDirectory = Path.Combine(baseDirectory, "TestImages");
        this.proxyHandler = new ImageMagickProxyHandler();
    }

    [DataRow("test.png")]
    [DataTestMethod]
    public async Task TestLocalFileFormat(string file)
    {
        if (Directory.Exists(this.proxyHandler.DefaultDownloadPath))
        {
            Directory.Delete(this.proxyHandler.DefaultDownloadPath, true);
        }

        if (Directory.Exists(this.proxyHandler.DefaultCachePath))
        {
            Directory.Delete(this.proxyHandler.DefaultCachePath, true);
        }

        Directory.CreateDirectory(this.proxyHandler.DefaultDownloadPath);
        Directory.CreateDirectory(this.proxyHandler.DefaultCachePath);

        var filepath = Path.Combine(this.baseDirectory, file);
        Assert.IsTrue(File.Exists(filepath));

        var imageOptions = new ImageTranscodeOptions();
        imageOptions.Format = ImageFormat.png;

        var result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));

        imageOptions.Format = ImageFormat.gif;

        result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));

        imageOptions.Format = ImageFormat.bmp;

        result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));

        imageOptions.Format = ImageFormat.jpg;

        result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));
    }

    [DataRow("https://user-images.githubusercontent.com/898335/167309884-29d91e46-90b3-401e-af65-b142e65ca41f.png")]
    [DataTestMethod]
    public async Task TestRemoteFileFormat(string uri)
    {
        if (Directory.Exists(this.proxyHandler.DefaultDownloadPath))
        {
            Directory.Delete(this.proxyHandler.DefaultDownloadPath, true);
        }

        if (Directory.Exists(this.proxyHandler.DefaultCachePath))
        {
            Directory.Delete(this.proxyHandler.DefaultCachePath, true);
        }

        Directory.CreateDirectory(this.proxyHandler.DefaultDownloadPath);
        Directory.CreateDirectory(this.proxyHandler.DefaultCachePath);

        var imageOptions = new ImageTranscodeOptions();
        imageOptions.Format = ImageFormat.png;

        var result = await this.proxyHandler.TranscodeImageAsync(uri, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));

        imageOptions.Format = ImageFormat.gif;

        result = await this.proxyHandler.TranscodeImageAsync(uri, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));

        imageOptions.Format = ImageFormat.bmp;

        result = await this.proxyHandler.TranscodeImageAsync(uri, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));

        imageOptions.Format = ImageFormat.jpg;

        result = await this.proxyHandler.TranscodeImageAsync(uri, imageOptions);

        Assert.IsTrue(File.Exists(result.FilePath));
    }
}