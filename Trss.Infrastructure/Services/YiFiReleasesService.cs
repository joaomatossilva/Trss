using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trss.Infrastructure.Services
{
    public class YiFiReleasesService : IReleasesService 
    {
        public async Task<Releases> GetReleases(string searchTitle, string quality, string sort, int page)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://yts.am/api/v2/list_movies.json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var queryString = "?limit=18";
                if (page > 1)
                {
                    queryString += "&set=" + page;
                }
                if (!string.IsNullOrEmpty(quality))
                {
                    queryString += "&quality=" + Uri.EscapeUriString(quality);
                }
                else
                {
                    queryString += "&quality=" + Uri.EscapeUriString("720p");
                }
                if (!string.IsNullOrEmpty(sort))
                {
                    queryString += "&sort=" + Uri.EscapeUriString(sort);
                }
                else
                {
                    queryString += "&sort=" + Uri.EscapeUriString("peers");
                }
                if (!string.IsNullOrEmpty(searchTitle))
                {
                    queryString += "&keywords=" + Uri.EscapeUriString(searchTitle);
                }

                var response = await client.GetAsync(queryString);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new UnderscorePropertyNamesContractResolver()
                };
                var yifiReleases = JsonConvert.DeserializeObject<YiFiResponse>(data, settings);
                var releases = new Releases
                {
                    MovieCount = yifiReleases.Data.MovieCount,
                    Movies = yifiReleases.Data.Movies.Select(GetRelease)
                };
                return releases;
            }

        }

        private Release GetRelease(YiFiRelease yiFiRelease)
        {
            var torrent = yiFiRelease.Torrents.First();
            var release = new Release
            {
                Url = torrent.Url,
                MovieID = torrent.Hash,
                CoverImage = yiFiRelease.MediumCoverImage,
                DateUploaded = yiFiRelease.DateUploaded,
                Genre = yiFiRelease.Genres.FirstOrDefault(),
                ImdbCode = yiFiRelease.ImdbCode,
                MovieTitleClean = yiFiRelease.TitleEnglish ?? yiFiRelease.Title,
                MovieYear = yiFiRelease.Year,
                Quality = torrent.Quality,
                ReleaseGroup = "yts",
                Size = torrent.Size,
                SizeByte = torrent.SizeBytes,
                TorrentHash = torrent.Hash,
                TorrentPeers = torrent.Peers,
                TorrentSeeds = torrent.Seeds
            };
            return release;
        }
    }
}
