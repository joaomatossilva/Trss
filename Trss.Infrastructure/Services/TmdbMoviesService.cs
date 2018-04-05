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
        private const string CastWidthFormat = "w45";
        private readonly TMDbClient _client;

        public TmdbMoviesService(TrssSettings settings)
        {
            _client = new TMDbClient(settings.TmdbApiKey);
            var config = _client.GetConfigAsync().Result; //async on constructure is dangerous
        }

        public async Task<Movies> Search(string text, int? page = null)
        {
            var pageNumber = page ?? 0;
            var results = await _client.SearchMovieAsync(text, pageNumber);
            return ToMovies(results);
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
            var result = await _client.GetMovieAsync(id, MovieMethods.Credits | MovieMethods.Videos);
            return ToMovie(result);
        }

        public async Task<Movie> GetMovieImdb(string imdbId)
        {
            var result = await _client.GetMovieAsync(imdbId, MovieMethods.Credits | MovieMethods.Videos);
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
                       Overview = movieResult.Overview,
                       PosterPath = _client.Config.Images.BaseUrl + ImageWidthFormat + movieResult.PosterPath
                   };
        }

        private Movie ToMovie(TMDbLib.Objects.Movies.Movie movieResult)
        {
            var movie = new Movie
            {
                MovieId = movieResult.Id,
                Title = movieResult.Title,
                OriginalTitle = movieResult.OriginalTitle,
                ReleaseDate = movieResult.ReleaseDate,
                ImdbId = movieResult.ImdbId,
                Overview = movieResult.Overview,
                PosterPath = _client.Config.Images.BaseUrl + ImageWidthFormat + movieResult.PosterPath,
            };

            if (movieResult.Credits != null)
            {
                movie.Cast = movieResult.Credits.Cast.Select(x => new Movie.CastItem
                {
                    Name = $"{x.Name} ({x.Character})",
                    Thumbnail = _client.Config.Images.BaseUrl + CastWidthFormat + x.ProfilePath
                }).Take(10).ToArray();
            }

            if (movieResult.Videos != null)
            {
                movie.Videos = movieResult.Videos.Results.Select(x => new Movie.Video
                {
                    Title = x.Name,
                    Address = x.Key,
                    Type = x.Type
                }).Take(10).ToArray();
            }

            return movie;
        }
        
    }
}
