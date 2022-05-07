// <copyright file="IndexPage.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Web4.Core;
using Web4.Handlebars;

namespace Web4.Index
{
    /// <summary>
    /// Index Page.
    /// </summary>
    public class IndexPage : IPage
    {
        private IReadOnlyList<ISite> sites;
        private HandlebarsTemplateRenderer templateRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPage"/> class.
        /// </summary>
        /// <param name="sites">List of Sites.</param>
        /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
        public IndexPage(IReadOnlyList<ISite> sites, HandlebarsTemplateRenderer templateRenderer)
        {
            this.sites = sites;
            this.templateRenderer = templateRenderer;
        }

        /// <inheritdoc/>
        public string Route => string.Empty;

        /// <inheritdoc/>
        public string PageName => "Web4 Index";

        /// <inheritdoc/>
        public RequestType RequestType => RequestType.GET;

        /// <inheritdoc/>
        public string TemplateName => "index.html.hbs";

        /// <inheritdoc/>
        public async Task Invoke(HttpContext context)
        {
            var sites = this.sites.GroupBy(n => n.Type);
            var content = this.templateRenderer.RenderHtml(this.TemplateName, sites);
            await context.WriteContentsWithEncodingAsync(content);
        }
    }
}
