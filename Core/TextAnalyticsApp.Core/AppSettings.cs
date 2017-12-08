using TextAnalyticsApp.Core.Contracts.Settings;

namespace TextAnalyticsApp.Core
{
    public class AppSettings : IAppSettings
    {
        public string ConnectionString { get; set; }
    }
}
