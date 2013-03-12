using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trss.Infrastructure;

namespace trss.Models
{
    public class TorrentViewModel
    {
        public Torrent Torrent { get; set; }
        public string Age { get; set; }
        public bool Downloaded { get; set; }
    }
}