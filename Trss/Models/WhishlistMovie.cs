using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Trss.Infrastructure;

namespace Trss.Models
{
    public class WishlistMovie : Movie
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? LastCheck { get; set; }
        public DateTime? FoundDate { get; set; }

        public static WishlistMovie FromMovie(Movie movie)
        {
            var whishlistMovie = new WishlistMovie
            {
                MovieId = movie.MovieId,
                ImdbId = movie.ImdbId,
                OriginalTitle = movie.OriginalTitle,
                Overview = movie.Overview,
                PosterPath = movie.PosterPath,
                ReleaseDate = movie.ReleaseDate,
                Tagline = movie.Tagline,
                Title = movie.Title
            };
            return whishlistMovie;
        }
    }
}
