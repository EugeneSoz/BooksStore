using System.Threading.Tasks;
using OnlineBooksStore.App.Contracts.Query;

namespace OnlineBooksStore.App.Handlers.Interfaces
{
    public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : BooksStore.App.Contracts.Query.Query
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}