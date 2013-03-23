using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using trss.Infrastructure;
using trss.Infrastructure.Sources;
using trss.Models;
using DateTimeExtensions;

namespace trss.Controllers
{
    [Authorize]
    public class HomeController : BootstrapBaseController
    {
        public ActionResult Index()
        {
            var torrentSource = new KickAssTorrentSource();
            var torrents = torrentSource.GetTorrents().ToList();

            var viewModel = ParseTorrents(torrents);
            return View(viewModel);
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult SearchMovie(string searchTerm)
        {
            var torrentSource = new KickAssTorrentSource();
            var torrents = torrentSource.SearchTorrents(searchTerm).ToList();
            var viewModel = ParseTorrents(torrents);
            return Json(viewModel);
        }

        private IEnumerable<TorrentViewModel> ParseTorrents(IEnumerable<Torrent> torrentList)
        {
            var selectedTorrents = Session.Load<SelectedTorrent>(torrentList.Select(t => t.Id))
               .Where(t => t != null)
               .ToList();

            return from t in torrentList
                   join s in selectedTorrents on t.Id equals s.Id into j
                   from j2 in j.DefaultIfEmpty()
                   select new TorrentViewModel
                              {
                                  Torrent = t,
                                  Age = t.PubDate.ToNaturalText(DateTime.Today),
                                  Downloaded = j2 != null
                              };
        }
        
        [HttpPost]
        public ActionResult Index(string id, string title)
        {

            var newSelectedTorrent = new SelectedTorrent
                                         {
                                             Date = DateTime.Now,
                                             Id = id,
                                             User = User.Identity.Name,
                                             Title = title
                                         };
            Session.Store(newSelectedTorrent);

            return RedirectToAction("Index");
        }
    }
}
