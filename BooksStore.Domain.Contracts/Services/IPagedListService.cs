using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.Services
{
    public interface IPagedListService<T>
    {
        PagedList<T> CreatePagedList(List<T> entities, int pagesCount, PageOptions options = null);
    }
}