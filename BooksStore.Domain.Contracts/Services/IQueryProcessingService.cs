using System;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.Services
{
    public interface IQueryProcessingService
    {
        (string conditions, bool isSearchOrFilterUsed) GenerateSqlQueryConditions(QueryConditions queryConditions);
    }
}