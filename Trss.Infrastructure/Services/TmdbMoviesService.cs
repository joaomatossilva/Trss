using System.Linq;
using System.Threading.Tasks;
using Formo;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace Trss.Infrastructure.Services
{
    public class TmdbMoviesService : IMoviesService
    {
        private const string ImageWidthFormat = "w185";
        private readonly TMDbClient _client;

        public TmdbMoviesService()
        {
            var config = new Configuration();
            _client = new TMDbClient(config.Get<string>("TmdbApiKey"));
            _client.GetConfig();
        }
        public async Task<Movies> NowPlaying()
        {
            return await Task.Run(() =>
                                  {
                                      var results = _client.GetMovieList(type: MovieListType.NowPlaying);
                                      return ToMovies(results);
                                  });
        }

        public async Task<Movies> TopRated()
        {
            return await Task.Run(() =>
                                  {
                                      var results = _client.GetMovieList(type: MovieListType.TopRated);
                                      return ToMovies(results);
                                  });
        }

        public async Task<Movie> GetMovie(int id)
        {
            return await Task.Run(() =>
            {
                var result = _client.GetMovie(id);
                return ToMovie(result);
            });
        }

        public async Task<Movie> GetMovieImdb(string imdbId)
        {
            return await Task.Run(() =>
            {
                var result = _client.GetMovie(imdbId);
                return ToMovie(result);
            });
        }

        private Movies ToMovies(SearchContainer<MovieResult> resultContainer)
        {
            return new Movies
                   {
                       TotalResults = resultContainer.TotalResults,
                       Results = resultContainer.Results
                           .Select(ToMovie)
                   };
        }

        private Movie ToMovie(MovieResult movieResult)
        {
            return new Movie
                   {
                       Id = movieResult.Id,
                       Title = movieResult.Title,
                       OriginalTitle = movieResult.OriginalTitle,
                       ReleaseDate = movieResult.ReleaseDate,
                       PosterPath = _client.Config.Images.BaseUrl + ImageWidthFormat + movieResult.PosterPath
                   };
        }

        private Movie ToMovie(TMDbLib.Objects.Movies.Movie movieResult)
        {
            return new Movie
            {
                Id = movieResult.Id,
                Title = movieResult.Title,
                OriginalTitle = movieResult.OriginalTitle,
                ReleaseDate = movieResult.ReleaseDate,
                ImdbId = movieResult.ImdbId,
                Tagline = movieResult.Tagline,
                Overview = movieResult.Overview,
                PosterPath = _client.Config.Images.BaseUrl + ImageWidthFormat + movieResult.PosterPath
            };
        }
        
    }
}
