using System.ComponentModel.DataAnnotations;

namespace BooksStore.App.Contracts.Command
{
    public abstract class CategoryCommand : BooksStore.App.Contracts.Command.Command
    {
        [Required(ErrorMessage = "Укажите название категории/подкатегории")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть не меньше 2 и не больше 100 символов")]
        public string Name { get; set; } = string.Empty;

        public long? ParentId { get; set; }
    }

    public sealed class CreateCategoryCommand : CategoryCommand { }
    public sealed class UpdateCategoryCommand : CategoryCommand { }
    public sealed class DeleteCategoryCommand : CategoryCommand { }
}