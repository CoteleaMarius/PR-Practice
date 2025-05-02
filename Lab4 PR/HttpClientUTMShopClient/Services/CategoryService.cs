using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using HttpClientUtmShopClient.Models;

namespace HttpClientUtmShopClient.Services
{
    public class CategoryService
    {
        private readonly HttpClient _client;

        public CategoryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Category>> GetCategoriesAsync() =>
            await _client.GetFromJsonAsync<List<Category>>("api/Category/categories");
        public async Task<List<Category>> GetCategoryAsync(long id) =>
            await _client.GetFromJsonAsync<List<Category>>($"api/Category/categories/{id}");

        public async Task CreateCategoryAsync(CreateCategoryDto category) =>
            await _client.PostAsJsonAsync("api/Category/categories", category);

        public async Task UpdateCategoryAsync(long id, CreateCategoryDto category) =>
            await _client.PutAsJsonAsync($"api/Category/{id}", category);

        public async Task DeleteCategoryAsync(long id) =>
            await _client.DeleteAsync($"api/Category/categories/{id}");
    }
}
