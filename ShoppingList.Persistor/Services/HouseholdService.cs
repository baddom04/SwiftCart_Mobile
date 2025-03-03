using ShoppingList.Core;
using ShoppingList.Core.Enums;
using ShoppingList.Persistor.DTO;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    public class HouseholdService(HttpClient client) : APIService(client), IHouseholdService
    {
        public async Task CreateNewHouseholdAsync(string name, string identifier, CancellationToken cancellationToken = default)
        {
            var payload = new { name, identifier };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("households", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<HouseholdRelationship> GetUserRelationShipAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/relationship", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            HouseholdRelationship? relationship = await response.Content.ReadFromJsonAsync<HouseholdRelationship>(cancellationToken);

            return relationship ?? throw new NullReferenceException();
        }

        public async Task DeleteHouseholdAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"households/{household_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<HouseholdsResponse> GetAllHouseholdsAsync(string search, int page, CancellationToken cancellationToken = default)
        {
            string endpoint = "households" + (!string.IsNullOrWhiteSpace(search) ? "/" + Uri.EscapeDataString(search) : "");

            HttpResponseMessage response = await _httpClient.GetAsync($"{endpoint}?per_page={NetworkSettings.HouseholdPerPage}&page={page}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            HouseholdsResponse? households = await response.Content.ReadFromJsonAsync<HouseholdsResponse>(cancellationToken);

            return households ?? throw new NullReferenceException(nameof(households));
        }

        public async Task<Household> GetHouseholdByIdAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Household? household = await response.Content.ReadFromJsonAsync<Household>(cancellationToken);

            return household ?? throw new NullReferenceException(nameof(household));
        }

        public async Task<IEnumerable<Household>> GetOwnedHouseholdsAsync(int user_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"users/{user_id}/households", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Household>? households = await response.Content.ReadFromJsonAsync<IEnumerable<Household>>(cancellationToken);

            return households ?? throw new NullReferenceException(nameof(households));
        }

        public async Task<IEnumerable<User>> GetUsersOfHouseholdAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/users", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<User>? users = await response.Content.ReadFromJsonAsync<IEnumerable<User>>(cancellationToken);

            return users ?? throw new NullReferenceException(nameof(users));
        }

        public async Task UpdateHouseholdAsync(int household_id, string? name, string? identifier, CancellationToken cancellationToken = default)
        {
            var payload = new { name, identifier };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"households/{household_id}", payload, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }
    }
}
