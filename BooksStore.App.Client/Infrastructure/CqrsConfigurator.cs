using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using Microsoft.Extensions.DependencyInjection;
using OnlineBooksStore.App.Handlers.Command;

namespace BooksStore.App.Client.Infrastructure
{
    internal static class CqrsConfigurator
    {
        internal static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
        {
            services.AddTransient<BookQueryHandler>();
            services.AddTransient<CategoryQueryHandler>();
            services.AddTransient<PublisherQueryHandler>();

            services.AddTransient<BookCommandHandler>();
            services.AddTransient<CategoryCommandHandler>();
            services.AddTransient<PublisherCommandHandler>();

            return services;
        }
    }
}