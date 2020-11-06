using AutoMapper;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Domain.Contracts.Models.Orders;
using BooksStore.Domain.Contracts.Models.Publishers;
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
            services.AddTransient<IPagedListService<CategoryResponse>, PagedListService<CategoryResponse>>();
            services.AddTransient<IPagedListService<PublisherResponse>, PagedListService<PublisherResponse>>();
            services.AddTransient<IPagedListService<Order>, PagedListService<Order>>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IPropertiesService, PropertiesService>();
            services.AddTransient<ISqlQueryProcessingService, SqlQueryProcessingService>();
            services.AddTransient<IBookSqlQueryProcessingService, BookSqlQueryProcessingService>();

            return services;
        }

        internal static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(ApplicationLayerMappings));
        }
    }
}