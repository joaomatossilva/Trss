using System.Collections.Generic;

namespace Trss.Models
{
    public class ReleasesListViewModel
    {
        private const int PagerMaxLenght = 5;

        public int TotalItemsFound { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int StartPage
        {
            get { return CurrentPage <= PagerMaxLenght / 2 ? 1 : CurrentPage - (PagerMaxLenght / 2); }
        }

        public int EndPage
        {
            get
            {
                var endPage = StartPage + PagerMaxLenght;
                if (endPage > TotalPages)
                {
                    endPage = TotalPages;
                }
                
                return endPage;
            }
        }

        public int TotalPages
        {
            get { return (TotalItemsFound / ItemsPerPage) + (TotalItemsFound % ItemsPerPage > 0 ? 1 : 0); }
        }

        public IEnumerable<ReleaseViewModel> List { get; set; }
    }
}