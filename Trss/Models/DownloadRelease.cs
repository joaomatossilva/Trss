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

    }
}