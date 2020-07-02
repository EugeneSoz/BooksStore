namespace OnlineBooksStore.App.Handlers.Interfaces
{
    public interface IQueryHandler<in TQuery, out TResult>
    {
        TResult Handle(TQuery query);
    }
}