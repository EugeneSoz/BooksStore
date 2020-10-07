using BooksStore.Domain.Contracts.Models.Pages;

namespace BooksStore.Domain.Contracts.Services
{
    public interface IQueryProcessingService
    {
        string GenerateSqlQueryConditions(QueryConditions queryConditions);
    }
}