using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Documents.Indexes;
using Trss.Infrastructure;
using Trss.Models;

namespace Trss.Indexes
{
    public class DownloadReleaseByHash : AbstractIndexCreationTask<DownloadRelease>
    {
        public DownloadReleaseByHash()
        {
            Map = releases => from release in releases select new {release.TorrentHash};
            Index(x => x.TorrentHash, FieldIndexing.Search);
        }
    }
}