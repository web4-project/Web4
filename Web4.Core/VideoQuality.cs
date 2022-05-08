// <copyright file="VideoQuality.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    /// <summary>
    /// Video Quality.
    /// </summary>
    public enum VideoQuality
    {
        /// <summary>
        /// Unknown Quality.
        /// </summary>
        Unknown,

        /// <summary>
        /// 4:3, 128×96.
        /// </summary>
        _96p,

        /// <summary>
        /// 4:3, 160×120.
        /// </summary>
        _120p,

        /// <summary>
        /// 11:9, 172x144.
        /// </summary>
        _144p,

        /// <summary>
        /// 16:9, 320x180.
        /// </summary>
        _180p,

        /// <summary>
        /// 4:3, 320x240.
        /// </summary>
        _240p,

        /// <summary>
        /// 30:17, 480x272.
        /// </summary>
        _272p,

        /// <summary>
        /// 4:3, 384x288.
        /// </summary>
        _288p,

        /// <summary>
        /// 4:3, 480x360.
        /// </summary>
        _360p,

        /// <summary>
        /// 4:3, 640x480.
        /// </summary>
        _480p,
    }
}
