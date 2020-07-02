using System.ComponentModel.DataAnnotations;

namespace BooksStore.App.Contracts.Command
{
    public abstract class PublisherCommand : BooksStore.App.Contracts.Command.Command
    {
        /// <summary>
        /// Gets or sets the publisher name.
        /// </summary>
        /// <value>
        /// The publisher name.
        /// </value>
        [Required(ErrorMessage = "Укажите название издательства")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть не меньше 2 и не больше 100 символов")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the publisher country of origin.
        /// </summary>
        /// <value>
        /// The country of origin.
        /// </value>
        [Required(ErrorMessage = "Укажите название страны нахождения издательства")]
        public string Country { get; set; }
    }

    public sealed class CreatePublisherCommand : PublisherCommand { }
    public sealed class UpdatePublisherCommand : PublisherCommand { }
    public sealed class DeletePublisherCommand : PublisherCommand { }
}