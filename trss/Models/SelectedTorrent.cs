using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trss.Models
{
    public class SelectedTorrent
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Title { get; set; }
    }
}