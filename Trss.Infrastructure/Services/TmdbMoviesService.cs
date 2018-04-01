using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Trss.Infrastructure.Services
{
    public class TmdbMoviesService : IMoviesService
    {
        private const string ImageWidthFormat = "w185";
        private readonly TMDbClient _client;

        public TmdbMoviesService(TrssSettings settings)
        {
            _client = new TMDbClient(settings.TmdbApiKey);
            var config = _client.GetConfigAsync().Result; //async on constructure is dangerous
        }
        public async Task<Movies> NowPlaying()
        {
            var results = await _client.GetMovieNowPlayingListAsync();
            return ToMovies(results);
        }

        public async Task<Movies> TopRated()
        {
            var results = await _client.GetMovieTopRatedListAsync();
            return ToMovies(results);
        }

        public async Task<Movie> GetMovie(int id)
        {
            var result = await _client.GetMovieAsync(id);
            return ToMovie(result);
        }

        public async Task<Movie> GetMovieImdb(string imdbId)
        {
            var result = await _client.GetMovieAsync(imdbId);
            return ToMovie(result);
        }

        private Movies ToMovies(SearchContainer<SearchMovie> resultContainer)
        {
            return new Movies
                   {
                       TotalResults = resultContainer.TotalResults,
                       Results = resultContainer.Results
                           .Select(ToMovie)
                   };
        }

        private Movie ToMovie(SearchMovie movieResult)
        {
            return new Movie
                   {
                       MovieId = movieResult.Id,
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
                MovieId = movieResult.Id,
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
