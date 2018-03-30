using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using Trss.Infrastructure;

namespace Trss.Models
{
    public class DownloadRelease : Release
    {
        [BsonId]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }

        public static DownloadRelease FromRelease(Release release)
        {
            var newDownloadRelease = new DownloadRelease
            {
                CoverImage = release.CoverImage,
                DateUploaded = release.DateUploaded,
                Genre = release.Genre,
                ImdbCode = release.ImdbCode,
                MovieID = release.MovieID,
                MovieTitleClean = release.MovieTitleClean,
                MovieYear = release.MovieYear,
                Quality = release.Quality,
                ReleaseGroup = release.ReleaseGroup,
                Size = release.Size,
                SizeByte = release.SizeByte,
                TorrentHash = release.TorrentHash,
                TorrentPeers = release.TorrentPeers,
                TorrentSeeds = release.TorrentSeeds,
                Url = release.Url
            };
            return newDownloadRelease;
        }
    }
}