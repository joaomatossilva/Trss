using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Raven.Client;
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
            releasesService = new YiFiReleasesService();
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
                }, JsonRequestBehavior.AllowGet); 
            }

            var viewmodel = Mapper.Map<IEnumerable<ReleaseViewModel>>(releases.MovieList);
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

            return Json(releasesListViewModel, JsonRequestBehavior.AllowGet);
        }

        private ReleaseViewModel UpdateViewModel(ReleaseViewModel viewModel, Release selected)
        {
            if (selected != null)
            {
                viewModel.Downloaded = true;
            }
            return viewModel;
        }

        public async Task<ActionResult> StoreRelease(DownloadRelease newSelectedRelease)
        {
            newSelectedRelease.Date = DateTime.Now;
            newSelectedRelease.UserId = User.Identity.GetUserId();
            Session.Store(newSelectedRelease);
            return Json(new {success = true});
        } 
    }
}