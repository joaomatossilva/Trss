using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trss.Infrastructure.Services
{
    public class TorrentsApiService : IReleasesService 
    {
        public async Task<Releases> GetReleases(string searchTitle, string quality, string sort, int page)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.torrentsapi.com/list");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                /*
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
                */

                var queryString = "?sort=seeds&quality=720p&page=1";

                var response = await client.GetAsync(queryString);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TorrentsApiResponse>(data);

                var releases = new Releases
                {
                    MovieCount = result.MovieList.Length,
                    Movies = result.MovieList.Select(x =>
                    {
                        var item = x.Items.FirstOrDefault();
                        if (item == null)
                        {
                            return null;
                        }
                        var release = new Release
                        {
                            CoverImage = x.Poster,
                            Genre = x.Genres != null ? x.Genres.FirstOrDefault() : null,
                            ImdbCode = x.Imdb,
                            MovieID = x.Id,
                            MovieTitleClean = x.Title,
                            MovieYear = x.Year,
                            Quality = item.Quality,
                            SizeByte = item.SizeBytes,
                            TorrentHash = item.Id,
                            TorrentPeers = item.TorrentPeers,
                            TorrentSeeds = item.TorrentSeeds,
                            Url = item.TorrentUrl
                        };
                        return release;
                    })
                    .Where(x => x != null)
                };
                return releases;
            }
        }
    }
}
