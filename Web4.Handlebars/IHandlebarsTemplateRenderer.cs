// <copyright file="IHandlebarsTemplateRenderer.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Web4.Core;

namespace Web4.Handlebars
{
    /// <summary>
    /// Handlebars Template Renderer.
    /// </summary>
    public interface IHandlebarsTemplateRenderer : ITemplateRenderer
    {
        /// <summary>
        /// Register Partial Template for the given Template Renderer.
        /// </summary>
        /// <param name="name">The name of the template.</param>
        /// <param name="templateHtml">The template HTML.</param>
        void RegisterPartialTemplate(string name, string templateHtml);
    }
}
