using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trss.Infrastructure.Services
{
    public class TorrentsApiResponse
    {
        public TorrentsApiMovie[] MovieList { get; set; }
    }

    public class TorrentsApiMovie
    {
        public string Id { get; set; }
        public string Tmdb { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public string Imdb { get; set; }
        public string Description { get; set; }
        [JsonProperty(propertyName: "poster_med")]
        public string Poster { get; set; }
        public string[] Genres { get; set; }
        public TorrentsApiItems[] Items { get; set; }
    }

    public class TorrentsApiItems
    {
        [JsonProperty(propertyName: "torrent_url")]
        public string TorrentUrl { get; set; }
        [JsonProperty(propertyName: "torrent_seeds")]
        public int TorrentSeeds { get; set; }
        [JsonProperty(propertyName: "torrent_peers")]
        public int TorrentPeers { get; set; }
        public string File { get; set; }
        public string Quality { get; set; }
        public int SizeBytes { get; set; }
        public string Id { get; set; }
    }
}
