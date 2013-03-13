using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using trss.Models;
using trss.Results;

namespace trss.Controllers
{
    [AllowAnonymous]
    public class FeedController : RavenDbController
    {
        //
        // GET: /Feed/

        public ActionResult Index()
        {
            var items = Session.Query<SelectedTorrent>()
                .OrderByDescending(t => t.Date)
                .Select(BuildSyndicationItem);

            var feed = new SyndicationFeed("Trss Feed", "TTrss Feed", null, items);

            var item = new SyndicationItem("Test Item",
                                    "This is the content for Test Item",
                                    new Uri("http://Contoso/ItemOne"),
                                    "TestItemID",
                                    DateTime.Now);

            return new RssActionResult() {Feed = feed};
        }

        private SyndicationItem BuildSyndicationItem(SelectedTorrent torrent)
        {
            var item = new SyndicationItem(torrent.Title, torrent.Title,
                                           new Uri("https://torcache.net/torrent/" + torrent.Id + ".torrent"),
                                           torrent.Id, torrent.Date);
            item.ElementExtensions.Add(
                new XElement("enclosure",
                             new XAttribute("type", "application/x-bittorrent"),
                             new XAttribute("length", "0"),
                             new XAttribute("url", "https://torcache.net/torrent/" + torrent.Id + ".torrent")
                    ).CreateReader()
                );

            return item;
        } 

    }
}
