using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trss.Infrastructure;

namespace Trss.Models
{
    public class DownloadRelease : Release
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }

    }
}