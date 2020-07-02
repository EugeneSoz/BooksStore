using System.ComponentModel.DataAnnotations;

namespace OnlineBooksStore.App.Contracts.Command
{
    public abstract class BookCommand : BooksStore.App.Contracts.Command.Command
    {
        //название книги
        [Required(ErrorMessage = "Укажите название книги")]
        [StringLength(250, MinimumLength = 2,
            ErrorMessage = "Название должно быть не меньше 2 и не больше 250 символов")]
        public string Title { get; set; } = string.Empty;

        //её авторы
        [Required(ErrorMessage = "Укажите автора или авторов")]
        public string Authors { get; set; } = string.Empty;

        //дата публикации
        [Range(1900, 9999, ErrorMessage = "Укажите год в диапазоне от 1900 до 9999г.")]
        public int Year { get; set; }

        //язык текста книги
        [Required(ErrorMessage = "Укажите язык книги")]
        public string Language { get; set; } = string.Empty;

        //количество страниц
        [Range(10, 5000, ErrorMessage = "Количество страниц не может быть менее 10 и больше 5000")]
        public int PageCount { get; set; }

        //описание
        [Required(ErrorMessage = "Добавьте какое либо описание книги")]
        [StringLength(int.MaxValue, MinimumLength = 2,
            ErrorMessage = "Название должно быть не меньше 2 и не больше 1000 символов")]
        public string Description { get; set; } = string.Empty;

        //текущая цена
        [Range(1, 100000, ErrorMessage = "Укажите стоимость товара")]
        public decimal RetailPrice { get; set; }

        //текущая цена
        [Range(1, 100000, ErrorMessage = "Укажите стоимость товара")]
        public decimal PurchasePrice { get; set; }

        //ссылка на изображение обложки
        public string BookCover { get; set; }
        public long? CategoryId { get; set; }
        public long? PublisherId { get; set; }
    }

    public sealed class CreateBookCommand : BookCommand { }
    public sealed class UpdateBookCommand : BookCommand { }
    public sealed class DeleteBookCommand : BookCommand { }
}