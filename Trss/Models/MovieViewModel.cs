using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trss.Infrastructure;

namespace Trss.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public bool IsOnWishlist { get; set; }
        public Guid? WishlistId { get; set; }
        public DateTime? AddedToWishlistDate { get; set; }
    }
}
