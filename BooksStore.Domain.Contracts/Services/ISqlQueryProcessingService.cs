using System;
using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.Services
{
    public interface ISqlQueryProcessingService
    {
        SqlQueryConditions GenerateSqlQueryConditions(QueryConditions queryConditions);
    }
}