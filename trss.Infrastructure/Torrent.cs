using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trss.Infrastructure
{
    public class Torrent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public int Seeders { get; set; }
        public int Leechers { get; set; }
        public long Size { get; set; }
    }
}
