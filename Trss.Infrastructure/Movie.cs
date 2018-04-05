using System;

namespace Trss.Infrastructure
{
    public class Movie
    {
        public Movie()
        {
            Videos = new Video[] {};
            Cast = new CastItem[] {};
        }

        public int MovieId { get; set; }
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public CastItem[] Cast { get; set; }
        public Video[] Videos { get; set; }

        public class CastItem
        {
            public string Name { get; set; }
            public string Thumbnail { get; set; }
        }

        public class Video
        {
            public string Address { get; set; }
            public string Type { get; set; }
            public string Title { get; set; }
            public string Thumbnail { get; set; }
        }
    }
}