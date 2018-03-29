using System;

namespace Trss.Infrastructure.Services
{
    public class YiFiRelease
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleEnglish { get; set; }
        public string ReleaseGroup { get; set; }
        public string YtTrailerCode { get; set; }
        public int Year { get; set; }
        public string ImdbCode { get; set; }
        public DateTime DateUploaded { get; set; }
        public string BackgroundImage { get; set; }
        public string MediumCoverImage { get; set; }
        public string[] Genres { get; set; }
        public string Url { get; set; }
        public YiFiTorrent[] Torrents { get; set; }
    }

    public class YiFiTorrent
    {
        public string Url { get; set; }
        public string Hash { get; set; }
        public string Quality { get; set; }
        public int Peers { get; set; }
        public int Seeds { get; set; }
        public string Size { get; set; }
        public long SizeBytes { get; set; }
    }
}