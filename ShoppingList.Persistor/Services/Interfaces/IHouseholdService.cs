﻿using ShoppingList.Core;

namespace ShoppingList.Persistor.Services.Interfaces;

public interface IHouseholdService
{
    Task CreateNewHouseholdAsync(string name, string identifier, CancellationToken cancellationToken = default);
    Task<IEnumerable<Household>> GetAllHouseholdsAsync(string search, int page, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersOfHouseholdAsync(int household_id, CancellationToken cancellationToken = default);
    Task<Household> GetHouseholdByIdAsync(int household_id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Household>> GetOwnedHouseholdsAsync(int user_id, CancellationToken cancellationToken = default);
    Task DeleteHouseholdAsync(int household_id, CancellationToken cancellationToken = default);
    Task UpdateHouseholdAsync(int household_id, string? name, string? identifier, CancellationToken cancellationToken = default);
}
