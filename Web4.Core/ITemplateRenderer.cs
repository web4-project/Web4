﻿// <copyright file="ITemplateRenderer.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    /// <summary>
    /// Template Renderer.
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// Gets the base directory for the templates.
        /// </summary>
        string BaseDirectory { get; }

        /// <summary>
        /// Render HTML Via the template.
        /// </summary>
        /// <param name="templatePath">Path to template.</param>
        /// <param name="viewModel">View Model for given template. To be rendered by the template renderer.</param>
        /// <returns>HTML String.</returns>
        string RenderHtml(string templatePath, object? viewModel);
    }
}
