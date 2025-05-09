﻿using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class ProductService(HttpClient client) : APIService(client), IProductService
    {
        public async Task<Product> CreateProductAsync(int segment_id, string name, string brand, string? description, uint price, CancellationToken cancellationToken = default)
        {
            var payload = new { name, brand, description, price };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"segments/{segment_id}/products", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Product? product = await response.Content.ReadFromJsonAsync<Product>(cancellationToken);

            return product ?? throw new NullReferenceException();
        }

        public async Task DeleteProductAsync(int segment_id, int product_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"segments/{segment_id}/products/{product_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Product> GetProductAsync(int product_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"products/{product_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Product? product = await response.Content.ReadFromJsonAsync<Product>(cancellationToken);

            return product ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int map_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"maps/{map_id}/products", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Product>? products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>(cancellationToken);

            return products ?? throw new NullReferenceException();
        }

        public async Task<Product> UpdateProductAsync(int segment_id, int product_id, string name, string brand, string? description, uint price, CancellationToken cancellationToken = default)
        {
            var payload = new { name, brand, description, price };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"segments/{segment_id}/products/{product_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Product? product = await response.Content.ReadFromJsonAsync<Product>(cancellationToken);

            return product ?? throw new NullReferenceException();
        }

        public async Task UpdateProductSegmentAsync(int old_segment_id, int product_id, int segment_id, CancellationToken cancellationToken = default)
        {
            var payload = new { segment_id };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"segments/{old_segment_id}/products/{product_id}/segment", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
