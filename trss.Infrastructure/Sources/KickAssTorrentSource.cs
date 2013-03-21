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
        private const string FeedUrl = @"http://kat.ph/movies/?rss=1&field=leechers&sorder=desc";

        public IEnumerable<Torrent> GetTorrents()
        {
            var request = HttpWebRequest.Create(FeedUrl);
            ((HttpWebRequest)request).AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);

            var mgr = new XmlNamespaceManager(new NameTable());
            mgr.AddNamespace("aaa", "http://tempuri.org");
            var ctx = new XmlParserContext(null, mgr, null, XmlSpace.Default);
            using (var reader = XmlReader.Create(request.GetResponse().GetResponseStream(), null, ctx))
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
