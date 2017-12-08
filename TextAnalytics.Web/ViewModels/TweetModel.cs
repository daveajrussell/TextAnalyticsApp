using System.Collections.Generic;

namespace TextAnalyticsApp.Web.ViewModels
{
    public class TweetModel
    {
        public List<string> Categories { get; set; } = new List<string>();
        public List<TweetData> Series { get; set; } = new List<TweetData>();
    }
}