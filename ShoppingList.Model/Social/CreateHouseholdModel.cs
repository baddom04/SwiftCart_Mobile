﻿using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class CreateHouseholdModel
    {
        private readonly IHouseholdService _householdService;

        public CreateHouseholdModel()
        {
            _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        }
        public async Task CreateHouseholdAsync(string name, string identifier)
        {
            await _householdService.CreateNewHouseholdAsync(name, identifier);
        }
        public async Task UpdateHouseholdAsync(int household_id, string name, string identifier)
        {
            await _householdService.UpdateHouseholdAsync(household_id, name, identifier);
        }
    }
}
