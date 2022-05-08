// <copyright file="AudioTranscodeOptions.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace Web4.Core
{
    /// <summary>
    /// Audio Transcode Options.
    /// </summary>
    public class AudioTranscodeOptions
    {
        /// <summary>
        /// Gets or sets the Audio Codec.
        /// </summary>
        public AudioCodec ACodec { get; set; } = AudioCodec.real_144;

        /// <summary>
        /// Gets or sets the Audio bitrate.
        /// </summary>
        public int ABitrate { get; set; } = 32;

        /// <summary>
        /// Gets or sets the Audio Sample Rate.
        /// </summary>
        public SampleRate SampleRate { get; set; } = SampleRate._8000;

        /// <summary>
        /// Gets or sets a value indicating whether to use the file cache.
        /// Defaults to true.
        /// </summary>
        public bool UseCache { get; set; } = true;

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.ACodec is AudioCodec.Unknown)
            {
                throw new ArgumentException("AudioCodec must not be Unknown");
            }

            return $"acodec={this.ACodec}&samplerate={this.SampleRate}&abitrate={this.ABitrate}&usecache={this.UseCache}";
        }

        public static AudioTranscodeOptions FromQueryString(IQueryCollection queryCollection)
        {
            var parameters = queryCollection.Keys.Cast<string>().ToDictionary(k => k, v => queryCollection[v].ToString());
            return AudioTranscodeOptions.FromQueryString(parameters);
        }

        public static AudioTranscodeOptions FromQueryString(Dictionary<string, string> query)
        {
            var options = new AudioTranscodeOptions();
            Enum.TryParse(typeof(AudioCodec), query.GetValueOrDefault("acodec"), out var audioCodec);
            Enum.TryParse(typeof(SampleRate), query.GetValueOrDefault("samplerate"), out var sampleRate);

            if (audioCodec is AudioCodec ac)
            {
                options.ACodec = ac;
            }

            if (sampleRate is SampleRate sr)
            {
                options.SampleRate = sr;
            }

            int.TryParse(query.GetValueOrDefault("abitrate"), out var abitrate);
            options.ABitrate = abitrate;

            return options;
        }
    }
}
