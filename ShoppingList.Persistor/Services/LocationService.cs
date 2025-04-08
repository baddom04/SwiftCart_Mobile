using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    internal class LocationService(HttpClient client) : APIService(client), ILocationService
    {
        public async Task<Location> CreateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default)
        {
            var payload = new { country, zip_code, city, street, detail };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"stores/{store_id}/location", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Location? location = await response.Content.ReadFromJsonAsync<Location>(cancellationToken);

            return location ?? throw new NullReferenceException();
        }

        public async Task DeleteLocationAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"stores/{store_id}/location", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Location> GetLocationAsync(int store_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"stores/{store_id}/location", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Location? location = await response.Content.ReadFromJsonAsync<Location>(cancellationToken);

            return location ?? throw new NullReferenceException();
        }
        private async Task<IEnumerable<string>> GetPossibleCountriesAsync(CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"locations/search/countries", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            PossibleLocations? possibleLocations = await response.Content.ReadFromJsonAsync<PossibleLocations>(cancellationToken);

            return possibleLocations?.Countries ?? throw new NullReferenceException();
        }
        private async Task<IEnumerable<string>> GetPossibleCitiesAsync(string country, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"locations/search/cities?country={Uri.EscapeDataString(country)}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            PossibleLocations? possibleLocations = await response.Content.ReadFromJsonAsync<PossibleLocations>(cancellationToken);

            return possibleLocations?.Cities ?? throw new NullReferenceException();
        }
        private async Task<IEnumerable<string>> GetPossibleStreetsAsync(string country, string city, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"locations/search/streets?country={Uri.EscapeDataString(country)}&city={Uri.EscapeDataString(city)}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            PossibleLocations? possibleLocations = await response.Content.ReadFromJsonAsync<PossibleLocations>(cancellationToken);

            return possibleLocations?.Streets ?? throw new NullReferenceException();
        }
        private async Task<IEnumerable<string>> GetPossibleDetailsAsync(string country, string city, string street, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"locations/search/details?country={Uri.EscapeDataString(country)}&city={Uri.EscapeDataString(city)}&street={Uri.EscapeDataString(street)}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            PossibleLocations? possibleLocations = await response.Content.ReadFromJsonAsync<PossibleLocations>(cancellationToken);

            return possibleLocations?.Details ?? throw new NullReferenceException();
        }


        public async Task<Location> UpdateLocationAsync(int store_id, string country, string zip_code, string city, string street, string? detail, CancellationToken cancellationToken = default)
        {
            var payload = new { country, zip_code, city, street, detail };

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"stores/{store_id}/location", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Location? location = await response.Content.ReadFromJsonAsync<Location>(cancellationToken);

            return location ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<string>> GetPossiblesAsync(CancellationToken cancellationToken = default, params string[] parameters)
        {
            return parameters.Length switch
            {
                0 => await GetPossibleCountriesAsync(cancellationToken),
                1 => await GetPossibleCitiesAsync(parameters[0], cancellationToken),
                2 => await GetPossibleStreetsAsync(parameters[1], parameters[0], cancellationToken),
                3 => await GetPossibleDetailsAsync(parameters[2], parameters[1], parameters[0], cancellationToken),
                _ => throw new InvalidDataException("The given parameters' length does not fall into the required range")
            };
        }
    }
}
