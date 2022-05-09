// <copyright file="ExampleSite.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using System.Reflection;
using Web4.Core;
using Web4.Handlebars;

namespace Web4.Index
{
    /// <summary>
    /// Example Site.
    /// </summary>
    public class ExampleSite : ISite
    {
        private HandlebarsTemplateRenderer templateRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleSite"/> class.
        /// </summary>
        public ExampleSite()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            var templatePath = Path.Combine(basePath, "Templates", "Examples");
            if (!Directory.Exists(templatePath))
            {
                throw new ArgumentException($"Could not find base template path, {templatePath}");
            }

            this.templateRenderer = new HandlebarsTemplateRenderer(templatePath);
        }

        /// <inheritdoc/>
        public string Route => "/examples";

        /// <inheritdoc/>
        public string Type => "Examples";

        /// <inheritdoc/>
        public string Name => "Web4 Examples";

        /// <inheritdoc/>
        public IReadOnlyList<IPage> Pages => new List<IPage>()
        {
            new IndexPage(this.templateRenderer),
            new ImagePage(this.templateRenderer),
            new ImageUploadPage(this.templateRenderer),
        };
    }
}
