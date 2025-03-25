using ShoppingList.Persistor.ServerResponseHandling;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal abstract class APIService(HttpClient httpClient)
    {
        protected readonly HttpClient _httpClient = httpClient;

        protected static async Task ValidateResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                ErrorResponse? errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken);
                if (errorResponse == null)
                    throw new NullReferenceException(nameof(errorResponse));

                throw new HttpRequestException($"{errorResponse.Error?.ToString()}");
            }
        }
    }
}