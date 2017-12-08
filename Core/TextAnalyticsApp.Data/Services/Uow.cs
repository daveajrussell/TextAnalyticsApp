using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using TextAnalyticsApp.Core;
using TextAnalyticsApp.Data.Abstractions;

namespace TextAnalyticsApp.Data.Services
{
    public class Uow : IUow, IDisposable
    {
        private CoreDbContext DbContext { get; }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        /* Core */
        public IRepository<User> Clients => GetStandardRepo<User>();
        public IRepository<Tweet> Tweets => GetStandardRepo<Tweet>();

        public Uow(IRepositoryProvider repositoryProvider, IDesignTimeDbContextFactory<CoreDbContext> context)
        {
            RepositoryProvider = repositoryProvider;

            DbContext = context.CreateDbContext(new string[] { });
        }

        protected T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>(DbContext);
        }

        protected IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>(DbContext);
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext?.Dispose();
            }
        }
    }
}
