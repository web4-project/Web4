// <copyright file="VideoCodec.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    /// <summary>
    /// Video Codecs.
    /// </summary>
    public enum VideoCodec
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Microsoft Video 1.
        /// Include with Windows 95 and support on Windows 3.1 with Video for Windows.
        /// </summary>
        msvideo1,

        /// <summary>
        /// Real Video 1.0.
        /// </summary>
        rv10,

        /// <summary>
        /// Real Video 2.0.
        /// </summary>
        rv20,

        /// <summary>
        /// Windows Media Video 1,
        /// Used with Windows Media Player 7+.
        /// </summary>
        wmv1,
    }
}
