namespace OnlineBooksStore.App.Handlers.Interfaces
{
    public interface ICommandHandler<in TCommand, out TResult> where TCommand : BooksStore.App.Contracts.Command.Command
    {
        TResult Handle(TCommand command);
    }
}