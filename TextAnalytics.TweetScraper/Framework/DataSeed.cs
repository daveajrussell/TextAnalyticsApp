using System.Linq;
using TextAnalyticsApp.Core;
using TextAnalyticsApp.Data.Abstractions;

namespace TextAnalyticsApp.TweetScraper.Framework
{
    public static class DataSeed
    {
        public static void SeedData(IUow uow)
        {
            if (!uow.Clients.GetAll().Any())
            {
                //uow.Clients.Add(new User
                //{
                //    Name = "",
                //    TwitterHandle = "@"
                //});

                uow.Commit();
            }
        }
    }
}