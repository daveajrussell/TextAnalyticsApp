using Microsoft.EntityFrameworkCore;
using TextAnalyticsApp.Core;
using TextAnalyticsApp.Data.Abstractions;

namespace TextAnalyticsApp.Data.Services
{
    public class CoreDbContext : DbContext, ICoreDbContext
    {
        /* Core */
        public DbSet<User> Clients { get; set; }
        public DbSet<Tweet> Tweets { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
