using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Trss.Infrastructure;
using Trss.Infrastructure.Services;
using Trss.Models;

namespace Trss.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMoviesService _moviesService;

        public MoviesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _moviesService = new TmdbMoviesService(new TrssSettings{ TmdbApiKey  = "ae7d3ccc0f2719f7e381467f65647d18" });
        }

        // GET: Movies
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> NowPlaying()
        {
            return PartialView("_Movies", await _moviesService.NowPlaying());
        }

        public async Task<ActionResult> TopRated()
        {
            return PartialView("_Movies", await _moviesService.TopRated());
        }

        public async Task<ActionResult> Movie(int id)
        {
            var movie = await _moviesService.GetMovie(id);
            var viewModel = await CreateMovieViewModel(movie);
            return View(viewModel);
        }

        public async Task<ActionResult> MovieImdb(string id)
        {
            var movie = await _moviesService.GetMovieImdb(id);
            var viewModel = await CreateMovieViewModel(movie);
            return View("Movie", viewModel);
        }

        private async Task<MovieViewModel> CreateMovieViewModel(Movie movie)
        {
            var viewModel = new MovieViewModel
            {
                Movie = movie
            };
            var wishlistMovie = await _dbContext.WishlistMovies.Find(x => x.MovieId == movie.MovieId).SingleOrDefaultAsync();
            if (wishlistMovie != null)
            {
                viewModel.IsOnWishlist = true;
                viewModel.AddedToWishlistDate = wishlistMovie.AddedDate;
                viewModel.WishlistId = wishlistMovie.Id;
            }

            return viewModel;
        }

        [HttpPost]
        public async Task<ActionResult> AddToWishlist(int id)
        {
            var movie = await _moviesService.GetMovie(id);
            var newWishList = WishlistMovie.FromMovie(movie);
            newWishList.AddedDate = DateTime.UtcNow;
            await _dbContext.WishlistMovies.InsertOneAsync(newWishList);
            return Json(new {success = true});
        }
    }
}