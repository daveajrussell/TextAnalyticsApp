using System.Collections.Generic;

namespace TextAnalyticsApp.Web.ViewModels
{
    public class TweetData
    {
        public string Name { get; set; }
        public List<double[]> Data { get; set; }
    }
}