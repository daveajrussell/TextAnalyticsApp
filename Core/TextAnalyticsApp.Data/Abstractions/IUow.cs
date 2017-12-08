using System.Threading.Tasks;
using TextAnalyticsApp.Core;

namespace TextAnalyticsApp.Data.Abstractions
{
    public interface IUow
    {
        void Commit();
        Task CommitAsync();

        /* Core */
        IRepository<User> Clients { get; }
        IRepository<Tweet> Tweets { get; }
    }
}