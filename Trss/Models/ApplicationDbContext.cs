using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDbGenericRepository;
using MongoDB.Driver;

namespace Trss.Models
{
    public class ApplicationDbContext : MongoDbContext
    {
        public ApplicationDbContext(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public ApplicationDbContext(string connectionString, string databaseName) : base(connectionString, databaseName)
        {
        }

        public IMongoCollection<DownloadRelease> DownloadReleases => Database.GetCollection<DownloadRelease>("downloadReleases");
        public IMongoCollection<WishlistMovie> WishlistMovies => Database.GetCollection<WishlistMovie>("wishlistMovies");
    }
}
