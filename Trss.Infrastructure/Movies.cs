using System.Collections.Generic;

namespace Trss.Infrastructure
{
    public class Movies
    {
        public int TotalResults { get; set; }
        public IEnumerable<Movie> Results { get; set; } 
    }
}