using System.Threading.Tasks;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Domain.Contracts.Services;

namespace OnlineBooksStore.Domain.Contracts.Services
{
    public interface IPublishersService
    {
        IPropertiesService Properties { get; }
        Task<PagedResponse<PublisherResponse>> GetPublishersAsync();
    }
}