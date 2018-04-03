using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trss.Infrastructure
{
    public interface IMoviesService
    {
        Task<Movies> Search(string text, int? page = null);
        Task<Movies> NowPlaying();
        Task<Movies> TopRated();
        Task<Movie> GetMovie(int id);
        Task<Movie> GetMovieImdb(string imdbId);
    }
}
