namespace HotelsSystem.Data
{
    public class PagedResult<T>
    {
        public PagedResult(int totalItems, decimal TotalOne, decimal TotalTwo, decimal TotalThree, decimal TotalFour,
            int pageNumber = 1,
            int pageSize = 10,
            int maxNavigationPages = 5,
            string sortColumn = "",
            string sortDirection = "Asc")
        {
            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);

            // Ensure actual page isn't out of range
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            else if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            int startPage;
            int endPage;
            if (totalPages <= maxNavigationPages)
            {
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                var maxPagesBeforeActualPage = (int)Math.Floor(maxNavigationPages / (decimal)2);
                var maxPagesAfterActualPage = (int)Math.Ceiling(maxNavigationPages / (decimal)2) - 1;
                if (pageNumber <= maxPagesBeforeActualPage)
                {
                    // Page at the start
                    startPage = 1;
                    endPage = maxNavigationPages;
                }
                else if (pageNumber + maxPagesAfterActualPage >= totalPages)
                {
                    // Page at the end
                    startPage = totalPages - maxNavigationPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // Page in the middle
                    startPage = pageNumber - maxPagesBeforeActualPage;
                    endPage = pageNumber + maxPagesAfterActualPage;
                }
            }

            // Create list of Page numbers
            var pageNumbers = Enumerable.Range(startPage, (endPage + 1) - startPage);

            StartPage = startPage;
            EndPage = endPage;
            PageNumber = pageNumber;
            PageNumbers = pageNumbers;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
            SortColumn = sortColumn;
            SortDirection = sortDirection;

            totalOne = TotalOne;
            totalTwo = TotalTwo;
            totalThree = TotalThree;
            totalFour = TotalFour;
        }
        public static PagedResult<T> EmptyPagedResult()
        {
            return new PagedResult<T>(0, 0, 0, 0, 0);
        }

        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// Total number of items to be paged
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Maximum number of page navigation links to display, default is 5
        /// </summary>
        public int MaxNavigationPages { get; private set; } = 5;

        /// <summary>
        /// Current active page
        /// </summary>
        public int PageNumber { get; private set; } = 1;

        /// <summary>
        /// Number of items per page, default is 10
        /// </summary>
        public int PageSize { get; private set; } = 10;

        public int TotalPages { get; private set; }
        public decimal totalOne { get; private set; }
        public decimal totalTwo { get; private set; }
        public decimal totalThree { get; private set; }
        public decimal totalFour { get; private set; }

        /// <summary>
        /// Start Page number
        /// </summary>
        public int StartPage { get; private set; }

        /// <summary>
        /// End Page number
        /// </summary>
        public int EndPage { get; private set; }

        public string SortColumn { get; private set; } = "";
        public string SortDirection { get; private set; } = "Asc";

        /// <summary>
        /// List of page numbers that we can loop
        /// </summary>
        public IEnumerable<int> PageNumbers { get; private set; }
    }
}