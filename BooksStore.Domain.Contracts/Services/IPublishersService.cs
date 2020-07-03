using System.Threading.Tasks;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Services;
using OnlineBooksStore.Domain.Contracts.Models.Publishers;

namespace OnlineBooksStore.Domain.Contracts.Services
{
    public interface IPublishersService
    {
        IPropertiesService Properties { get; }
        Task<PagedResponse<PublisherResponse>> GetPublishersAsync();
    }
}