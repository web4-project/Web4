// <copyright file="SampleRate.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    /// <summary>
    /// Audio Sample Rate.
    /// </summary>
    public enum SampleRate
    {
        Unknown,

        /// <summary>
        /// 8000kHz, Very low quality.
        /// </summary>
        _8000,

        /// <summary>
        /// 11025kHz, vLow Quality.
        /// </summary>
        _11025,

        /// <summary>
        /// 22050kHz, Medium Quality.
        /// </summary>
        _22050,

        /// <summary>
        /// 32000kHz, Highish Quality.
        /// </summary>
        _32000,

        /// <summary>
        /// 44100kHz, High Quality.
        /// </summary>
        _44100,

        /// <summary>
        /// 48000kHz, DVD quality.
        /// </summary>
        _48000,
    }
}
