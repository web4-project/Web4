// <copyright file="VideoTranscodeOptions.cs" company="Web4 Team">
// Copyright (c) Web4 Team. All rights reserved.
// </copyright>

namespace Web4.Core
{
    /// <summary>
    /// Video Transcoder Options.
    /// </summary>
    public class VideoTranscodeOptions
    {
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public VideoFormat Format { get; set; } = VideoFormat.None;

        /// <summary>
        /// Gets or sets the Video Codec.
        /// </summary>
        public VideoCodec VCodec { get; set; } = VideoCodec.Unknown;

        /// <summary>
        /// Gets or sets the Video Bitrate.
        /// </summary>
        public int VBitrate { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Audio Codec.
        /// </summary>
        public AudioCodec ACodec { get; set; } = AudioCodec.Unknown;

        /// <summary>
        /// Gets or sets the Audio bitrate.
        /// </summary>
        public int ABitrate { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Audio Sample Rate.
        /// </summary>
        public SampleRate SampleRate { get; set; } = SampleRate.Unknown;

        /// <summary>
        /// Gets or sets the video quality.
        /// </summary>
        public VideoQuality VideoQuality { get; set; } = VideoQuality.Unknown;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; } = 0;

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value to scale the video down by.
        /// Ignores width and height parameter.
        /// </summary>
        public int ScaleDownBy { get; set; } = 0;

        /// <summary>
        /// Gets or sets the frames per second for the video.
        /// </summary>
        public int FramesPerSecond { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to use the file cache.
        /// Defaults to true.
        /// </summary>
        public bool UseCache { get; set; } = true;

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.Format is VideoFormat.None)
            {
                throw new ArgumentException("VideoFormat must not be None");
            }

            if (this.VCodec is VideoCodec.Unknown)
            {
                throw new ArgumentException("VideoCodec must not be Unknown");
            }

            if (this.ACodec is AudioCodec.Unknown)
            {
                throw new ArgumentException("AudioCodec must not be Unknown");
            }

            if (this.VideoQuality is VideoQuality.Unknown)
            {
                throw new ArgumentException("VideoQuality must not be Unknown");
            }

            return $"format={this.Format}&vcodec={this.VCodec}&acodec={this.ACodec}&videoquality={this.VideoQuality}&samplerate={this.SampleRate}&abitrate={this.ABitrate}&vbitrate={this.VBitrate}&width={this.Width}&height={this.Height}&scaledownby={this.ScaleDownBy}&framespersecond={this.FramesPerSecond}&usecache={this.UseCache}";
        }

        public static VideoTranscodeOptions FromQueryString(Dictionary<string, string> query)
        {
            var options = new VideoTranscodeOptions();
            Enum.TryParse(typeof(VideoFormat), query.GetValueOrDefault("format"), out var videoFormat);
            Enum.TryParse(typeof(VideoCodec), query.GetValueOrDefault("vcodec"), out var videoCodec);
            Enum.TryParse(typeof(AudioCodec), query.GetValueOrDefault("acodec"), out var audioCodec);
            Enum.TryParse(typeof(SampleRate), query.GetValueOrDefault("samplerate"), out var sampleRate);
            Enum.TryParse(typeof(VideoQuality), query.GetValueOrDefault("videoquality"), out var videoQuality);

            if (videoFormat is VideoFormat format)
            {
                options.Format = format;
            }

            if (videoCodec is VideoCodec vc)
            {
                options.VCodec = vc;
            }

            if (audioCodec is AudioCodec ac)
            {
                options.ACodec = ac;
            }

            if (sampleRate is SampleRate sr)
            {
                options.SampleRate = sr;
            }

            if (videoQuality is VideoQuality vq)
            {
                options.VideoQuality = vq;
            }

            int.TryParse(query.GetValueOrDefault("width"), out var width);
            int.TryParse(query.GetValueOrDefault("height"), out var height);
            int.TryParse(query.GetValueOrDefault("abitrate"), out var abitrate);
            int.TryParse(query.GetValueOrDefault("vbitrate"), out var vbitrate);
            int.TryParse(query.GetValueOrDefault("scaledownby"), out var scaledownby);
            int.TryParse(query.GetValueOrDefault("framespersecond"), out var framespersecond);

            options.Width = width;
            options.Height = height;
            options.ScaleDownBy = scaledownby;
            options.FramesPerSecond = framespersecond;
            options.ABitrate = abitrate;
            options.VBitrate = vbitrate;

            return options;
        }
    }
}
