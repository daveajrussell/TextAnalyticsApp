using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TextAnalyticsApp.Data.Helpers
{
    public class RepositoryFactories
    {
        public RepositoryFactories()
        {
            _repositoryFactories = GetLightningAppFactories();
        }

        private IDictionary<Type, Func<DbContext, object>> GetLightningAppFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {
                //{ typeof(IRepo), dbContext => new ConcreteRepo(dbContext) }
            };
        }

        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            _repositoryFactories.TryGetValue(typeof(T), out Func<DbContext, object> factory);

            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new EFRepository<T>(dbContext);
        }
    }
}
