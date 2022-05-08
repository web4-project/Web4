// <copyright file="AudioCodec.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    /// <summary>
    /// Audio Codecs.
    /// </summary>
    public enum AudioCodec
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// PCM unsigned 8-bit.
        /// </summary>
        pcm_u8,

        /// <summary>
        /// RealAudio 1.0 (14.4K) (codec ra_144).
        /// </summary>
        real_144,

        /// <summary>
        /// ADPCM Microsoft.
        /// </summary>
        adpcm_ms,

        /// <summary>
        /// Windows Media Audio 1,
        /// Used with Windows Media Player 7+.
        /// </summary>
        wmav1,
    }
}
