using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Trss.Infrastructure;
using Trss.Infrastructure.Services;
using Trss.Models;

namespace Trss.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IReleasesService _releasesService;

        public WishlistController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _releasesService = new YiFiReleasesService();
        }

        public async Task<IActionResult> Index()
        {
            var items = await _dbContext.WishlistMovies.Find(FilterDefinition<WishlistMovie>.Empty)
                .SortByDescending(x => x.AddedDate)
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _dbContext.WishlistMovies.Find(x => x.Id == id).FirstOrDefaultAsync();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, IFormCollection form)
        {
            await _dbContext.WishlistMovies.FindOneAndDeleteAsync(x => x.Id == id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Process()
        {
            var topWishlistItem = await _dbContext.WishlistMovies.Find(x => x.FoundDate == null)
                .SortBy(x => x.LastCheck)
                .FirstOrDefaultAsync();

            topWishlistItem.LastCheck = DateTime.UtcNow;

            var filter = Builders<WishlistMovie>.Filter.Eq(x => x.Id, topWishlistItem.Id);
            var updateLastCheckDate = Builders<WishlistMovie>.Update.Set(x => x.LastCheck, topWishlistItem.LastCheck);
            await _dbContext.WishlistMovies.FindOneAndUpdateAsync(filter, updateLastCheckDate);

            var releases = await _releasesService.GetReleases(topWishlistItem.Title, "720p", null, 1);
            var release = releases?.Movies.FirstOrDefault();
            if (release != null)
            {
                topWishlistItem.FoundDate = DateTime.UtcNow;
                var updateFoundDate = Builders<WishlistMovie>.Update.Set(x => x.FoundDate, topWishlistItem.FoundDate);
                await _dbContext.WishlistMovies.FindOneAndUpdateAsync(filter, updateFoundDate);
                // Send email notification

                var downloadRelease = DownloadRelease.FromRelease(release);
                downloadRelease.Date = DateTime.UtcNow;
                await _dbContext.DownloadReleases.InsertOneAsync(downloadRelease);
            }

            return Json(new {success = true});
        }
    }
}