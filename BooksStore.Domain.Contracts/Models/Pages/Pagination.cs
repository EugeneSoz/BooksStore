using System.Collections.Generic;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int LeftBoundary { get; set; }
        public int RightBoundary { get; set; }
        public List<int> PageNumbers { get; set; }
    }
}
