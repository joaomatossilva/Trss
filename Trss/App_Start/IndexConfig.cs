using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Abstractions.Indexing;
using Raven.Client.Document;

namespace Trss
{
    public class IndexConfig
    {
        public static void CreateIndexes(DocumentStore docStore)
        {
            /*docStore.DatabaseCommands.PutIndex("ReleasesByHash", new IndexDefinition
                                                                 {
                                                                     Map =
                                                                         "from release in docs.Release select new { release.TorrentHash }",
                                                                     Indexes = {{"TorrentHash", FieldIndexing.Analyzed}}
                                                                 });
             */
        }
    }
}