using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace trss.Infrastructure.Sources
{
    public class KickAssTorrentSource : ITorrentSource
    {
        private const string SearchUrl = @"http://kat.ph/usearch/{0}%20category:movies/?rss=1";
        private const string FeedUrl = @"http://kat.ph/movies/";
        private const string FeedSufix = @"?rss=1&field=seeders&sorder=desc";

        public IEnumerable<Torrent> GetTorrents()
        {
            var firstPage = ParseUrl(FeedUrl + FeedSufix);
            var secondPage = ParseUrl(FeedUrl + "2/" + FeedSufix);
            return firstPage.Union(secondPage).ToList();
        }

        public IEnumerable<Torrent> SearchTorrents(string searchTerm)
        {
            return ParseUrl(string.Format(SearchUrl, searchTerm));
        }

        private IEnumerable<Torrent> ParseUrl(string url)
        {
            var request = HttpWebRequest.Create(url);
            ((HttpWebRequest)request).AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);

            using (var reader = XmlReader.Create(request.GetResponse().GetResponseStream()))
            {
                var document = XDocument.Load(reader);
                return document.Descendants("channel").Descendants("item")
                    .Select(BuildTorrentFromRssItem)
                    .Where(t => FilterTorrent(t.Title));
            }
        } 

        private bool FilterTorrent(string title)
        {
            
            if (title.IndexOf("dvdrip", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
            if (title.IndexOf("brrip", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
            if (title.IndexOf("720p", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
            if (title.IndexOf("1080p", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
            if (title.IndexOf("DVDSCR", 0, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return true;
            }
            return false;
        }

        private Torrent BuildTorrentFromRssItem(XElement item)
        {
            var torrent = new Torrent();
            torrent.Id = item.Element("{http://xmlns.ezrss.it/0.1/}infoHash") != null ? item.Element("{http://xmlns.ezrss.it/0.1/}infoHash").Value : "";
            torrent.Title = item.Element("title") != null ? item.Element("title").Value : "";
            torrent.Description = item.Element("description") != null ? item.Element("description").Value : "";
            torrent.Seeders = item.Element("{http://xmlns.ezrss.it/0.1/}seeds") != null ? int.Parse(item.Element("{http://xmlns.ezrss.it/0.1/}seeds").Value) : 0;
            torrent.Leechers = item.Element("{http://xmlns.ezrss.it/0.1/}peers") != null ? int.Parse(item.Element("{http://xmlns.ezrss.it/0.1/}peers").Value) : 0;
            torrent.Size = item.Element("{http://xmlns.ezrss.it/0.1/}contentLength") != null ? long.Parse(item.Element("{http://xmlns.ezrss.it/0.1/}contentLength").Value) : 0;
            torrent.PubDate = item.Element("pubDate") != null ? DateTime.Parse(item.Element("pubDate").Value) : DateTime.MinValue;
            return torrent;
        }
    }
}
