using System;

namespace Trss.Infrastructure
{
    public class Release
    {
        public string MovieID { get; set; }
        public string MovieTitleClean { get; set; }
        public string ReleaseGroup { get; set; }
        public int MovieYear { get; set; }
        public string ImdbCode { get; set; }
        public DateTime DateUploaded { get; set; }
        public int TorrentSeeds { get; set; }
        public int TorrentPeers { get; set; }
        public long SizeByte { get; set; }
        public string Size { get; set; }
        public string TorrentHash { get; set; }
        public string CoverImage { get; set; }
        public string Quality { get; set; }
        public string Genre { get; set; }
        public string Url { get; set; }
    }
}