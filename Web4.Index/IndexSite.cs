// <copyright file="IndexSite.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Reflection;
using Web4.Core;
using Web4.Handlebars;

namespace Web4.Index
{
    /// <summary>
    /// Index Site.
    /// </summary>
    public class IndexSite : ISite
    {
        private HandlebarsTemplateRenderer templateRenderer;

        private IReadOnlyList<ISite> sites;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSite"/> class.
        /// </summary>
        /// <param name="sites">List of sites.</param>
        public IndexSite(IReadOnlyList<ISite> sites)
        {
            this.sites = sites;
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            var templatePath = Path.Combine(basePath, "Templates", "Home");
            if (!Directory.Exists(templatePath))
            {
                throw new ArgumentException($"Could not find base template path, {templatePath}");
            }

            this.templateRenderer = new HandlebarsTemplateRenderer(templatePath);
        }

        /// <inheritdoc/>
        public string Route => "/";

        /// <inheritdoc/>
        public string Type => "Index";

        /// <inheritdoc/>
        public string Name => "Web4";

        /// <inheritdoc/>
        public IReadOnlyList<IPage> Pages => new List<IPage>() { new IndexPage(this.sites, this.templateRenderer) };
    }
}
