using System.Threading.Tasks;

namespace BooksStore.App.Handlers.Interfaces
{
    public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : BooksStore.App.Contracts.Query.Query
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}