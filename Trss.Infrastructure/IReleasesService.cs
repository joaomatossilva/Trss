using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trss.Infrastructure
{
    public interface IReleasesService
    {
        Task<Releases> GetReleases(string searchTitle, string quality, string sort, int page);
        Task<Release> GetRelease(string id);
    }
}
