using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Repositories;
using BooksStore.Persistence.Repositories.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksStore.App.Client.Infrastructure
{
    internal static class PersistenceLayerConfigurator
    {
        internal static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider => new ConnectionProvider(configuration["ConnectionStrings:StoreConnection"]));

            services.AddTransient<IBooksRepository, BooksRepository>();
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<IPublishersRepository, PublishersRepository>();

            return services;
        }
    }
}