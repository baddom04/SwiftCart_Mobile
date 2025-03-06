using ShoppingList.Core;
using ShoppingList.Persistor.Services.Interfaces;
using System.Net.Http.Json;

namespace ShoppingList.Persistor.Services
{
    public class ApplicationService(HttpClient client) : APIService(client), IApplicationService
    {
        public async Task AcceptUserAsync(int application_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"applications/{application_id}", new { }, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task ApplyAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"households/{household_id}/applications", new { }, cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task DeleteApplicationAsync(int application_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"applications/{application_id}", cancellationToken);

            await ValidateResponse(response, cancellationToken);
        }

        public async Task<Application> GetApplicationByDataAsync(int household_id, int user_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"users/{user_id}/households/{household_id}/application", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            Application? application = await response.Content.ReadFromJsonAsync<Application>(cancellationToken);

            return application ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Household>> GetAppliedToHouseholdsAsync(int user_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"users/{user_id}/applications/households", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Household>? households = await response.Content.ReadFromJsonAsync<IEnumerable<Household>>(cancellationToken);
            return households ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<User>> GetAppliedUsersAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/applications/users", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<User>? users = await response.Content.ReadFromJsonAsync<IEnumerable<User>>(cancellationToken);
            return users ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Application>> GetHouseholdApplicationsAsync(int household_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"households/{household_id}/applications", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Application>? users = await response.Content.ReadFromJsonAsync<IEnumerable<Application>>(cancellationToken);
            return users ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Application>> GetUserApplicationsAsync(int user_id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"users/{user_id}/applications", cancellationToken);

            await ValidateResponse(response, cancellationToken);

            IEnumerable<Application>? users = await response.Content.ReadFromJsonAsync<IEnumerable<Application>>(cancellationToken);
            return users ?? throw new NullReferenceException();
        }
    }
}
