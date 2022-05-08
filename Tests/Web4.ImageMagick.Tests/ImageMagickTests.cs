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
            Directory.CreateDirectory(this.proxyHandler.DefaultDownloadPath);
        }

        if (Directory.Exists(this.proxyHandler.DefaultCachePath))
        {
            Directory.Delete(this.proxyHandler.DefaultCachePath, true);
            Directory.CreateDirectory(this.proxyHandler.DefaultCachePath);
        }

        var filepath = Path.Combine(this.baseDirectory, file);
        Assert.IsTrue(File.Exists(filepath));

        var imageOptions = new ImageTranscodeOptions();
        imageOptions.Format = ImageFormat.png;

        var result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result));

        imageOptions.Format = ImageFormat.gif;

        result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result));

        imageOptions.Format = ImageFormat.bmp;

        result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result));

        imageOptions.Format = ImageFormat.jpg;

        result = await this.proxyHandler.TranscodeImageAsync(filepath, imageOptions);

        Assert.IsTrue(File.Exists(result));
    }
}