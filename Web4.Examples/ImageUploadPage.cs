// <copyright file="ImageUploadPage.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Web4.Core;
using Web4.Handlebars;

namespace Web4.Index
{
    public class ImageUploadPage : IPage
    {
        private HandlebarsTemplateRenderer templateRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUploadPage"/> class.
        /// </summary>
        /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
        public ImageUploadPage(HandlebarsTemplateRenderer templateRenderer)
        {
            this.templateRenderer = templateRenderer;
        }

        /// <inheritdoc/>
        public string Route => "/images";

        /// <inheritdoc/>
        public string PageName => "Web4 Examples - Image Debug POST";

        /// <inheritdoc/>
        public RequestType RequestType => RequestType.POST;

        /// <inheritdoc/>
        public string TemplateName => "images.html.hbs";

        /// <inheritdoc/>
        public async Task Invoke(HttpContext context)
        {
            var vm = new ImageUploadDebugViewModel();
            var files = context.Request.Form.Files;
            if (files.Any())
            {
                var file = files.First();

                // TODO: Add to local storage.
                // TODO: Add Remote Upload option (Imgur?)
            }

            var content = this.templateRenderer.RenderHtml(this.TemplateName, vm);
            await context.WriteContentsWithEncodingAsync(content);
        }

        private class ImageUploadDebugViewModel
        {
            public bool HasImage => !string.IsNullOrEmpty(this.ImageUrl);

            public string? ImageUrl { get; set; }
        }
    }
}