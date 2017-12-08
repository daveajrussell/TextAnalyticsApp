using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TextAnalyticsApp.Data.Abstractions;

namespace TextAnalyticsApp.Data.Helpers
{
    public class RepositoryProvider : IRepositoryProvider
    {
        protected Dictionary<Type, object> Repositories { get; }

        private readonly RepositoryFactories _repositoryFactories;

        public RepositoryProvider(RepositoryFactories repositoryFactories)
        {
            _repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }

        public IRepository<T> GetRepositoryForEntityType<T>(DbContext context) where T : class
        {
            return GetRepository<IRepository<T>>(context, _repositoryFactories.GetRepositoryFactoryForEntityType<T>());
        }

        public virtual T GetRepository<T>(DbContext context, Func<DbContext, object> factory = null) where T : class
        {
			Repositories.TryGetValue(typeof(T), out object repoObj);

			if (repoObj != null)
                return (T)repoObj;

            return MakeRepository<T>(factory, context);
        }

		protected virtual T MakeRepository<T>(Func<DbContext, object> factory, DbContext dbContext)
        {
            var f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();

			if (f == null)
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);

            var repo = (T)f(dbContext);

			Repositories[typeof(T)] = repo;

			return repo;
        }
    }
}