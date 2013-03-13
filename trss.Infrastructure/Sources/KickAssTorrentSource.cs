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
        private const string FeedUrl = @"http://kat.ph/movies/?rss=1&field=seeders&sorder=desc";

        public IEnumerable<Torrent> GetTorrents()
        {
            var request = HttpWebRequest.Create(FeedUrl);
            ((HttpWebRequest)request).AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);

            var mgr = new XmlNamespaceManager(new NameTable());
            mgr.AddNamespace("content", "http://tempuri.org");
            var ctx = new XmlParserContext(null, mgr, null, XmlSpace.Default);
            using (var reader = XmlReader.Create(request.GetResponse().GetResponseStream(), null, ctx))
            {
                var document = XDocument.Load(reader);
                return document.Descendants("channel").Descendants("item").Select(BuildTorrentFromRssItem);
            }
        }

        private Torrent BuildTorrentFromRssItem(XElement item)
        {
            var torrent = new Torrent();
            torrent.Id = item.Element("hash") != null ? item.Element("hash").Value : "";
            torrent.Title = item.Element("title") != null ? item.Element("title").Value : "";
            torrent.Description = item.Element("description") != null ? item.Element("description").Value : "";
            torrent.Seeders = item.Element("seeds") != null ? int.Parse(item.Element("seeds").Value) : 0;
            torrent.Leechers = item.Element("leechs") != null ? int.Parse(item.Element("leechs").Value) : 0;
            torrent.Size = item.Element("size") != null ? long.Parse(item.Element("size").Value) : 0;
            torrent.PubDate = item.Element("pubDate") != null ? DateTime.Parse(item.Element("pubDate").Value) : DateTime.MinValue;
            return torrent;
        }
    }
}
