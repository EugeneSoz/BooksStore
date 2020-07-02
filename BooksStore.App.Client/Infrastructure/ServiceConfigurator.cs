using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Services;
using BooksStore.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BooksStore.App.Client.Infrastructure
{
    internal static class ServiceConfigurator
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPagedListService<BookResponse>, PagedListService<BookResponse>>();

            return services;
        }
    }
}