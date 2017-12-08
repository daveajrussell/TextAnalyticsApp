using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TextAnalyticsApp.Data.Services;

namespace TextAnalyticsApp.Data.Factories
{
    public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        public CoreDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CoreDbContext>();

            var connectionString = configuration.GetSection("AppSettings:ConnectionString").Value;

            builder.UseSqlServer(connectionString);

            return new CoreDbContext(builder.Options);
        }
    }
}