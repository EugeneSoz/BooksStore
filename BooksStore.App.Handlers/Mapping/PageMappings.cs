using System.Collections.Generic;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.App.Handlers.Mapping
{
    public static class PageMappings
    {
        public static PagedResponse<T> MapPagedResponse<T>(this PagedList<T> response)
        {
            var numbers = new List<int>();
            for (int i = response.LeftBoundary; i <= response.RightBoundary; i++)
            {
                numbers.Add(i);
            }

            return new PagedResponse<T>
            {
                Entities = response.Entities ?? new List<T>(),
                Pagination = new Pagination
                {
                    CurrentPage = response.CurrentPage,
                    PageSize = response.PageSize,
                    TotalPages = response.TotalPages,
                    HasPreviousPage = response.HasPreviousPage,
                    HasNextPage = response.HasNextPage,
                    LeftBoundary = response.LeftBoundary,
                    RightBoundary = response.RightBoundary,
                    PageNumbers = numbers
                }
            };
        }
    }
}