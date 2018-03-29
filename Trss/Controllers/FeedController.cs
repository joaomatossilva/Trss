using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using MongoDB.Driver;
using Trss.Models;
using WilderMinds.RssSyndication;

namespace Trss.Controllers
{
    [AllowAnonymous]
    public class FeedController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FeedController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Feed
        public async Task<ActionResult> Index(string user)
        {
            var findOptions = new FindOptions<DownloadRelease>
            {
                Sort = Builders<DownloadRelease>.Sort.Ascending("Date")
            };
            var releases = await _dbContext.DownloadReleases
                .FindAsync(t => t.UserId == user, findOptions);

            var feed = new Feed
            {
                Title = "Trss Feed",
                Link = new Uri("https://trss.azurewebsites.net/feed"),
                Description = "TTrss Feed"
            };

            feed.Items = releases.ToEnumerable().Select(x => BuildItem(x)).ToList();
            return new ContentResult {Content = feed.Serialize(), ContentType = "application/rss+xml"};
        }

        private Item BuildItem(DownloadRelease torrent)
        {
            var link = torrent.Url ?? "https://torcache.net/torrent/" + torrent.TorrentHash + ".torrent";
            var item = new Item
            {
                Title = torrent.MovieTitleClean,
                Permalink = torrent.TorrentHash,
                PublishDate = torrent.Date,
                Link = new Uri(link)
            };
            var enclosure = new Enclosure();
            enclosure.Values["type"] = "application/x-bittorrent";
            enclosure.Values["length"] = "0";
            enclosure.Values["url"] = link;
            item.Enclosures.Add(enclosure);
            return item;
        }
    }
}