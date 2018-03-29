using System;
using System.Collections.Generic;
using System.Text;

namespace Trss.Infrastructure.Services
{
    public class YiFiReleasesList
    {
        public int MovieCount { get; set; }
        public IEnumerable<YiFiRelease> Movies { get; set; }
    }
}