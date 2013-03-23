using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace trss.Infrastructure
{
    public interface ITorrentSource
    {
        IEnumerable<Torrent> GetTorrents();
        IEnumerable<Torrent> SearchTorrents(string searchTerm);
    }
}
