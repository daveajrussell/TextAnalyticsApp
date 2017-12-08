using System;

namespace TextAnalyticsApp.Core
{
    public class Tweet
    {
        public int Id { get; set; }

        public string FullText { get; set; }

        public DateTime Datestamp { get; set; }

        public double Sentiment { get; set; }

        public virtual User Client { get; set; }

        public int ClientId { get; set; }
    }
}