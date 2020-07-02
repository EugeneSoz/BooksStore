using System.Threading.Tasks;

namespace OnlineBooksStore.App.Handlers.Interfaces
{
    public interface IAsyncCommandHandler<in TCommand> where TCommand : BooksStore.App.Contracts.Command.Command
    {
        Task HandleAsync(TCommand command);
    }
}