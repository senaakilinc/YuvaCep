using System.Text.Json.Serialization;
using YuvaCep.Mobile.Enums;

namespace YuvaCep.Mobile.Dtos
{
    public class ActivityChartDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("theme")]
        public ChartTheme Theme { get; set; }

        [JsonPropertyName("isCompletedToday")]
        public bool IsCompletedToday { get; set; }

        [JsonPropertyName("totalCompletedCount")]
        public int TotalCompletedCount { get; set; }
    }
}