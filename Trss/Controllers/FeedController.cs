using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trss.ActionResults;
using Trss.Models;

namespace Trss.Controllers
{
    [AllowAnonymous]
    public class FeedController : RavenDbBaseController
    {
        // GET: Feed
        public ActionResult Index(string user)
        {
            var items = Session.Query<DownloadRelease>()
                .Where(t => t.UserId == user)
                .OrderByDescending(t => t.Date)
                .Select(BuildSyndicationItem);

            var feed = new SyndicationFeed("Trss Feed", "TTrss Feed", null, items);
            return new RssActionResult() { Feed = feed };
        }

        private SyndicationItem BuildSyndicationItem(DownloadRelease torrent)
        {
            var item = new SyndicationItem(torrent.MovieTitleClean, torrent.MovieTitleClean,
                                           new Uri("https://torcache.net/torrent/" + torrent.TorrentHash + ".torrent"),
                                           torrent.TorrentHash, torrent.Date);
            item.ElementExtensions.Add(
                new XElement("enclosure",
                             new XAttribute("type", "application/x-bittorrent"),
                             new XAttribute("length", "0"),
                             new XAttribute("url", "https://torcache.net/torrent/" + torrent.TorrentHash + ".torrent")
                    ).CreateReader()
                );

            return item;
        } 
    }
}