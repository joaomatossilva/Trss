using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Trss.Infrastructure;
using Trss.Infrastructure.Services;

namespace Trss.Controllers
{
    public class MoviesController : Controller
    {
        private IMoviesService moviesService;

        public MoviesController()
        {
            moviesService = new TmdbMoviesService(new TrssSettings{ TmdbApiKey  = "ae7d3ccc0f2719f7e381467f65647d18" });
        }

        // GET: Movies
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> NowPlaying()
        {
            return PartialView("_Movies", await moviesService.NowPlaying());
        }

        public async Task<ActionResult> TopRated()
        {
            return PartialView("_Movies", await moviesService.TopRated());
        }

        public async Task<ActionResult> Movie(int id)
        {
            return View(await moviesService.GetMovie(id));
        }

        public async Task<ActionResult> MovieImdb(string id)
        {
            return View("Movie", await moviesService.GetMovieImdb(id));
        }
    }
}