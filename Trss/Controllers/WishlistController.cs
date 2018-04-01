using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Trss.Models;

namespace Trss.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public WishlistController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _dbContext.WishlistMovies.Find(FilterDefinition<WishlistMovie>.Empty)
                .SortByDescending(x => x.AddedDate)
                .ToListAsync();
            return View(items);
        }
    }
}