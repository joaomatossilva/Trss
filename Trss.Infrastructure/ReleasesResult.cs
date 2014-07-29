using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trss.Infrastructure
{
    public class Releases
    {
        public int MovieCount { get; set; }
        public IEnumerable<Release> MovieList { get; set; } 
    }
}
