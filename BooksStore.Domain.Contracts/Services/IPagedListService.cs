using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.Services
{
    public interface IPagedListService<T>
    {
        PagedList<T> CreatePagedList(IEnumerable<T> entities, int pagesCount, QueryConditions conditions = null);
    }
}