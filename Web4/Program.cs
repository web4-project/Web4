// <copyright file="Program.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Text;
using Web4;
using Web4.Core;
using Web4.ImageMagick;
using Web4.ImageSharp;
using Web4.Index;
using Web4.FFMpeg;

// In order for encodings to work when writing out HTML for some providers
// Like Shift-JIS, we need to register the encoding handler.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

SiteSetup.Sites.Add(new ExampleSite());

SiteSetup.Sites.Add(new IndexSite(SiteSetup.Sites.ToList()));

builder.WebHost.UseUrls("http://*:5001");

var app = builder.Build();

HttpFileClient client = new HttpFileClient();

IImageProxyHandler imageProxy;

// Magick.NET does not have an M1 port yet, so use ImageSharp instead.
if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
{
    imageProxy = new ImageSharpProxyHandler(client);
}
else
{
    imageProxy = new ImageMagickProxyHandler(client);
}

IAudioProxyHandler audioProxy = new FFMpegAudioProxyHandler(client);

IVideoProxyHandler videoProxy = new FFMpegVideoProxyHandler(client);

app.MapGet("/proxy/image/{*remander}", imageProxy.InvokeImageProxy);

app.MapGet("/proxy/audio/{*remander}", audioProxy.InvokeAudioProxy);

app.MapGet("/proxy/video/{*remander}", videoProxy.InvokeVideoProxy);

foreach (var site in SiteSetup.Sites)
{
    foreach (var page in site.Pages)
    {
        string endpointName;

        // Index of the site.
        if (site.Route == "/" & page.Route == "/")
        {
            endpointName = "/";
        }

        // Index of the feature page.
        else if (site.Route != "/" && page.Route == string.Empty)
        {
            endpointName = site.Route;
        }

        // Nested page within the feature page.
        else
        {
            endpointName = site.Route + page.Route;
        }

        switch (page.RequestType)
        {
            case RequestType.GET:
                app.MapGet(endpointName, page.Invoke);
                break;
            case RequestType.POST:
                app.MapPost(endpointName, page.Invoke);
                break;
        }
    }
}

// TODO: Allow for other ports.
app.Run("http://*:5001");
