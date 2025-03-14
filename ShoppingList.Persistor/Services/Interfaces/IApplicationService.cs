using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces
{
    public interface IApplicationService
    {
        Task ApplyAsync(int household_id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Application>> GetUserApplicationsAsync(int user_id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Household>> GetAppliedToHouseholdsAsync(int user_id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Application>> GetHouseholdApplicationsAsync(int household_id, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAppliedUsersAsync(int household_id, CancellationToken cancellationToken = default);
        Task AcceptUserAsync(int application_id, CancellationToken cancellationToken = default);
        Task DeleteApplicationAsync(int application_id, CancellationToken cancellationToken = default);
        Task<Application> GetApplicationByDataAsync(int household_id, int user_id, CancellationToken cancellationToken = default);
    }
}
