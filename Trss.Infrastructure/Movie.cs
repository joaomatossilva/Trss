using System;

namespace Trss.Infrastructure
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }

    }
}