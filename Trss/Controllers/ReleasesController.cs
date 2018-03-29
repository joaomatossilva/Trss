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
    public class ReleasesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IReleasesService _releasesService;

        public ReleasesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _releasesService = new YiFiReleasesService();
        }

        // GET: Releases
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetReleases(string searchTitle, string quality, string sort = null, int page = 1)
        {
            var releases = await _releasesService.GetReleases(searchTitle, quality, sort, page);
            if (releases.MovieCount == 0)
            {
                return Json(new ReleasesListViewModel
                {
                    CurrentPage = page,
                    TotalItemsFound = releases.MovieCount,
                    ItemsPerPage = 18,
                }); 
            }

            //var viewmodel = Mapper.Map<IEnumerable<ReleaseViewModel>>(releases.MovieList);
            var viewmodel = releases.Movies.Select(m => new ReleaseViewModel
                                                           {
                                                               CoverImage = m.CoverImage,
                                                               DateUploaded = m.DateUploaded,
                                                               Genre = m.Genre,
                                                               ImdbCode = m.ImdbCode,
                                                               MovieID = m.MovieID,
                                                               MovieTitleClean = m.MovieTitleClean,
                                                               MovieYear = m.MovieYear,
                                                               Quality = m.Quality,
                                                               ReleaseGroup = m.ReleaseGroup,
                                                               Size = m.Size,
                                                               SizeByte = m.SizeByte,
                                                               TorrentHash = m.TorrentHash,
                                                               TorrentPeers = m.TorrentPeers,
                                                               TorrentSeeds = m.TorrentSeeds,
                                                               Url = m.Url
                                                           });

            var filterDefinition = new FilterDefinitionBuilder<DownloadRelease>();
            var filter = filterDefinition.In(x => x.TorrentHash, viewmodel.Select(v => v.TorrentHash));
            var selectedReleasesQuery = await _dbContext.DownloadReleases.FindAsync(filter);
            var selectedReleases = await selectedReleasesQuery.ToListAsync();

            var filledViewModel = from t in viewmodel
                join s in selectedReleases on t.TorrentHash equals s.TorrentHash into j
                from j2 in j.DefaultIfEmpty()
                select UpdateViewModel(t, j2);

            var releasesListViewModel = new ReleasesListViewModel
                                        {
                                            CurrentPage = page,
                                            TotalItemsFound = releases.MovieCount,
                                            ItemsPerPage = 18,
                                            List = filledViewModel
                                        };

            return Json(releasesListViewModel);
        }

        private ReleaseViewModel UpdateViewModel(ReleaseViewModel viewModel, Release selected)
        {
            if (selected != null)
            {
                viewModel.Downloaded = true;
            }
            return viewModel;
        }

        private async Task StoreReleaseInternal(DownloadRelease newSelectedRelease)
        {
            newSelectedRelease.Date = DateTime.Now;
            //newSelectedRelease.UserId = User.Identity.GetUserId();
            await _dbContext.DownloadReleases.InsertOneAsync(newSelectedRelease);
        }

        public async Task<ActionResult> StoreRelease(DownloadRelease newSelectedRelease)
        {
            await StoreReleaseInternal(newSelectedRelease);
            return Json(new {success = true});
        }

        public ActionResult AddHash()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddHash(DownloadRelease newSelectedRelease)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await StoreReleaseInternal(newSelectedRelease);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(newSelectedRelease);
        }
    }
}