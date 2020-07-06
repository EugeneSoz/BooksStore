using System.ComponentModel.DataAnnotations;

namespace BooksStore.App.Contracts.Command
{
    public class CreateCustomerCommand : Command
    {
        [Required(ErrorMessage = "Укажите ваше имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите ваш адрес доставки")]
        public string Address { get; set; }
        public string State { get; set; }
        [Required(ErrorMessage = "Укажите индекс адреса доставки")]
        public string ZipCode { get; set; }
    }
}