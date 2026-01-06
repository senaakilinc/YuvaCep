using System.Text.Json.Serialization;
using YuvaCep.Mobile.Enums;

namespace YuvaCep.Mobile.Dtos
{
    public class CreateActivityChartDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("theme")]
        public ChartTheme Theme { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }
    }
}