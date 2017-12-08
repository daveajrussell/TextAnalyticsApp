using System;
using System.Collections.Generic;
using System.Linq;
using TextAnalyticsApp.Data.Helpers;
using TextAnalyticsApp.TweetScraper.Framework;
using Microsoft.ProjectOxford.Text.Sentiment;
using Tweetinvi;
using Tweetinvi.Parameters;
using Tweet = TextAnalyticsApp.Core.Tweet;
using System.Configuration;
using static TextAnalyticsApp.TweetScraper.Framework.ConfigurationConstants;
using TextAnalyticsApp.Data.Services;
using TextAnalyticsApp.Data.Abstractions;
using TextAnalyticsApp.Data.Factories;

namespace TextAnalyticsApp.TweetScraper
{
    public class Program
    {
        private static IUow _uow;

        public static void Main(string[] args)
        {
            _uow = new Uow(new RepositoryProvider(new RepositoryFactories()), new CoreDbContextFactory());

            DataSeed.SeedData(_uow);

            var clients = _uow.Clients.GetAll().ToList();

            Auth.SetUserCredentials(ConfigurationManager.AppSettings[ConsumerKey], ConfigurationManager.AppSettings[ConsumerSecret], ConfigurationManager.AppSettings[AccessToken], ConfigurationManager.AppSettings[AccessSecret]);

            var since = DateTime.UtcNow.AddDays(-1);

            foreach (var client in clients)
            {
                Console.WriteLine($"Processing tweets for {client.Name} since {since:g}");

                // Pretty basic search, just get everything in the last 24 hours
                var searchParameter = new SearchTweetsParameters(client.TwitterHandle)
                {
                    Since = since
                };

                var tweets = Search.SearchTweets(searchParameter);

                ProcessTweetSentiments(client, since, tweets?.Select(x => x.FullText).ToList() ?? new List<string>());
            }
        }

        public static void ProcessTweetSentiments(Core.User client, DateTime since, IList<string> tweets)
        {
            if (!tweets.Any()) return;

            var request = new SentimentRequest();
            var id = 1;

            foreach (var tweet in tweets)
            {
                request.Documents.Add(new SentimentDocument
                {
                    Id = id.ToString(),
                    Text = tweet,
                    Language = "en"
                });

                id++;
            }

            var sentimentClient = new SentimentClient(ConfigurationManager.AppSettings[TextAnalyticsApiKey]);

            var response = sentimentClient.GetSentiment(request);

            foreach (var doc in response.Documents)
            {
                _uow.Tweets.Add(new Tweet
                {
                    ClientId = client.Id,
                    Datestamp = since,
                    FullText = request.Documents.FirstOrDefault(o => o.Id == doc.Id).Text,
                    Sentiment = doc.Score
                });

                Console.WriteLine("Score: {0}%", (doc.Score * 100));
            }

            _uow.Commit();
        }
    }
}