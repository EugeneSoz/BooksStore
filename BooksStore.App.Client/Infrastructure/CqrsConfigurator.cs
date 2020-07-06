using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using Microsoft.Extensions.DependencyInjection;

namespace BooksStore.App.Client.Infrastructure
{
    internal static class CqrsConfigurator
    {
        internal static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
        {
            services.AddTransient<BookQueryHandler>();
            services.AddTransient<CategoryQueryHandler>();
            services.AddTransient<PublisherQueryHandler>();
            services.AddTransient<CartQueryHandler>();

            services.AddTransient<BookCommandHandler>();
            services.AddTransient<CategoryCommandHandler>();
            services.AddTransient<PublisherCommandHandler>();
            services.AddTransient<CartCommandHandler>();
            services.AddTransient<OrderCommandHandler>();

            return services;
        }
    }
}