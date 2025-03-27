﻿using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Core;
using ShoppingList.Persistor;
using ShoppingList.Persistor.DTO;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Social
{
    public class SocialPanelModel
    {
        private readonly IHouseholdService _householdService;

        public IEnumerable<Household>? LoadedHouseholds { get; private set; }
        public int MaxPage { get; private set; } = 1;

        public SocialPanelModel()
        {
            _householdService = AppServiceProvider.Services.GetRequiredService<IHouseholdService>();
        }

        public async Task<IEnumerable<Household>> SearchHouseholdsAsync(string search, int page)
        {
            PaginatedResponse<Household> response = await _householdService.GetAllHouseholdsAsync(search, page);

            LoadedHouseholds = response.QueryResult;

            MaxPage = response.Meta.MaxPages;

            return LoadedHouseholds;
        }
    }
}
