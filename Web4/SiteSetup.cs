// <copyright file="SiteSetup.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Web4.Core;

namespace Web4
{
    /// <summary>
    /// Site Setup.
    /// </summary>
    public static class SiteSetup
    {
        /// <summary>
        /// Gets the lists of sites.
        /// </summary>
        public static IList<ISite> Sites { get; } = new List<ISite>();
    }
}