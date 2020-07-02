using OnlineBooksStore.Domain.Contracts.Models.Database;

namespace BooksStore.App.Contracts.Command
{
    public abstract class TablesCommand : BooksStore.App.Contracts.Command.Command
    {
        public AreaGroup AreaGroup { get; set; }
    }

    public sealed class CreateTablesCommand : TablesCommand { }
    public sealed class DeleteTablesCommand : TablesCommand { }
    public sealed class FillTablesCommand : TablesCommand { }
}