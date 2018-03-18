using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Raven.Client;
using Raven.Client.Documents;
using Sparrow;
using Trss.Infrastructure;
using Trss.Infrastructure.Services;
using Trss.Models;

namespace Trss.Controllers
{
    public class ReleasesController : RavenDbBaseController
    {
        private IReleasesService releasesService;

        public ReleasesController()
        {
            releasesService = new TorrentsApiService();
        }

        // GET: Releases
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetReleases(string searchTitle, string quality, string sort = null, int page = 1)
        {
            var releases = await releasesService.GetReleases(searchTitle, quality, sort, page);
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
            var viewmodel = releases.MovieList.Select(m => new ReleaseViewModel
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
                                                               TorrentSeeds = m.TorrentSeeds
                                                           });
            var selectedReleases = Session.Query<DownloadRelease>("DownloadReleaseByHash")
                .Search(x => x.TorrentHash, string.Join(" ", viewmodel.Select(v => v.TorrentHash))).ToList();
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

        private void StoreReleaseInternal(DownloadRelease newSelectedRelease)
        {
            newSelectedRelease.Date = DateTime.Now;
            //newSelectedRelease.UserId = User.Identity.GetUserId();
            Session.Store(newSelectedRelease);
        }

        public async Task<ActionResult> StoreRelease(DownloadRelease newSelectedRelease)
        {
            StoreReleaseInternal(newSelectedRelease);
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
                    StoreReleaseInternal(newSelectedRelease);
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