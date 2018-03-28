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

            var items = releases.ToEnumerable()
                .Select(BuildSyndicationItem);

            var sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() {Async = true, Indent = true}))
            {
                var writer = new RssFeedWriter(xmlWriter);
                await writer.WriteTitle("Trss Feed");
                await writer.WriteDescription("TTrss Feed");

                foreach (var item in items)
                {
                    await writer.Write(item);
                }
                await writer.Flush();
            }

            return new ContentResult {Content = sw.ToString(), ContentType = "application/rss+xml"};
        }

        private SyndicationItem BuildSyndicationItem(DownloadRelease torrent)
        {
            var item = new SyndicationItem()
            {
                Title = torrent.MovieTitleClean,
                Description = torrent.MovieTitleClean,
                Id = torrent.TorrentHash,
                Published = torrent.Date
            };
            item.AddLink(new SyndicationLink(new Uri("https://torcache.net/torrent/" + torrent.TorrentHash + ".torrent")));

            /*item.
            
            item.ElementExtensions.Add(
                new XElement("enclosure",
                             new XAttribute("type", "application/x-bittorrent"),
                             new XAttribute("length", "0"),
                             new XAttribute("url", "https://torcache.net/torrent/" + torrent.TorrentHash + ".torrent")
                    ).CreateReader()
                );
                */
            return item;
        } 
    }
}