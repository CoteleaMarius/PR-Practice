using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HttpClientUtmShopClient.Models;

namespace HttpClientUtmShopClient.Services
{
    public class ProductService
    {
        private readonly HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ProductShortDto>> GetProductsInCategoryAsync(long categoryId) =>
            await _client.GetFromJsonAsync<List<ProductShortDto>>($"api/Category/categories/{categoryId}/products");

        public async Task AddProductToCategoryAsync(long categoryId, ProductShortDto product) =>
            await _client.PostAsJsonAsync($"api/Category/categories/{categoryId}/products", product);
    }
}
