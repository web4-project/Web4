// <copyright file="ImagePage.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Web4.Core;
using Web4.Handlebars;

namespace Web4.Index
{
    /// <summary>
    /// Image Page.
    /// </summary>
    public class ImagePage : IPage
    {
        private HandlebarsTemplateRenderer templateRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePage"/> class.
        /// </summary>
        /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
        public ImagePage(HandlebarsTemplateRenderer templateRenderer)
        {
            this.templateRenderer = templateRenderer;
        }

        /// <inheritdoc/>
        public string Route => "/images";

        /// <inheritdoc/>
        public string PageName => "Web4 Examples - Image Debug";

        /// <inheritdoc/>
        public RequestType RequestType => RequestType.GET;

        /// <inheritdoc/>
        public string TemplateName => "images.html.hbs";

        /// <inheritdoc/>
        public async Task Invoke(HttpContext context)
        {
            var content = this.templateRenderer.RenderHtml(this.TemplateName, null);
            await context.WriteContentsWithEncodingAsync(content);
        }
    }
}