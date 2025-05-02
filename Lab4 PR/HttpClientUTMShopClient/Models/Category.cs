using System.Text.Json.Serialization;
namespace HttpClientUtmShopClient.Models
{
    public class Category
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("itemsCount")]
        public int ItemsCount { get; set; }
    }
}
