using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Orders;
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
            services.AddTransient<IPagedListService<Order>, PagedListService<Order>>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IPropertiesService, PropertiesService>();

            return services;
        }
    }
}