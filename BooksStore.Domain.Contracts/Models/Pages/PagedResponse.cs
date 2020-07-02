using System.Collections.Generic;

namespace BooksStore.Domain.Contracts.Models.Pages
{
    public class PagedResponse<T>
    {
        public List<T> Entities { get; set; }
        public Pagination Pagination { get; set; }
    }
}
