using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using TextAnalyticsApp.Data.Abstractions;
using TextAnalyticsApp.Data.Factories;
using TextAnalyticsApp.Data.Services;

namespace TextAnalyticsApp.Data.Helpers
{
    public static class ServiceCollectionExtensionMethods
    {
        public static void AddDataLayer(this IServiceCollection services)
        {
            services.AddScoped<RepositoryFactories>();
            services.AddScoped<IDesignTimeDbContextFactory<CoreDbContext>, CoreDbContextFactory>();
            services.AddTransient<IRepositoryProvider, RepositoryProvider>();
            services.AddTransient<IUow, Uow>();
        }
    }
}
