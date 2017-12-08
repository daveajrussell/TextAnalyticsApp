using Microsoft.EntityFrameworkCore;
using TextAnalyticsApp.Core;

namespace TextAnalyticsApp.Data.Abstractions
{
    public interface ICoreDbContext
    {
        /* Core */
        DbSet<User> Clients { get; set; }
        DbSet<Tweet> Tweets { get; }
    }
}
