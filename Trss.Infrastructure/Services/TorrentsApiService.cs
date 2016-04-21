using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                return await response.Content.ReadAsAsync<Releases>();
            }
        }
    }
}
