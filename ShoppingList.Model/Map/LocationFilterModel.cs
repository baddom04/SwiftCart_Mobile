﻿using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;

namespace ShoppingList.Model.Map
{
    public class LocationFilterModel
    {
        public IReadOnlyDictionary<LocationProperty, LocationPropertyFilter> LocationFilters { get; }

        private readonly ILocationService _locationService = AppServiceProvider.Services.GetRequiredService<ILocationService>();
        public LocationFilterModel() 
        {
            Dictionary<LocationProperty, LocationPropertyFilter> _locationFilters = [];
            LocationProperty? parentType = null;
            foreach (LocationProperty type in Enum.GetValues(typeof(LocationProperty)))
            {
                _locationFilters.Add(
                    type, 
                    new LocationPropertyFilter(
                        _locationService, 
                        !parentType.HasValue 
                            ? null 
                            : _locationFilters[parentType.Value]
                        ));

                parentType = type;
            }
            List<LocationProperty> reverseLocationProperties = Enum.GetValues(typeof(LocationProperty)).Cast<LocationProperty>().ToList();
            reverseLocationProperties.Reverse();
            LocationProperty? childType = null;
            foreach (LocationProperty type in reverseLocationProperties)
            {
                _locationFilters[type].Child = childType.HasValue 
                    ? _locationFilters[childType.Value] 
                    : null;
                childType = type;
            }
            LocationFilters = _locationFilters;
        }
    }
    public class LocationPropertyFilter
    {
        private IEnumerable<string> _possibles = [];
        public IEnumerable<string> Possibles
        {
            get { return _possibles; }
            internal set { _possibles = value; PossiblesChanged?.Invoke(); }
        }

        public LocationPropertyFilter? Parent { get; }
        public LocationPropertyFilter? Child { get; internal set; }
        public string? Search { get; internal set; }

        private readonly ILocationService _locationService;
        public event Action? PossiblesChanged;
        public LocationPropertyFilter(ILocationService locationService, LocationPropertyFilter? parent)
        {
            _locationService = locationService;
            Parent = parent;
            if (Parent is not null)
                Parent.PossiblesChanged += async () => await GetPossiblesAsync(Parent.Search);
        }
        private IEnumerable<string?> GetParentSearches()
        {
            var current = Parent;
            while (current != null)
            {
                yield return current.Search;
                current = current.Parent;
            }
        }
        public async Task GetPossiblesAsync(string? search)
        {
            if(Parent is not null)
                Parent.Search = search;

            IEnumerable<string?> parentSearches = GetParentSearches();
            Possibles = Parent is not null && (parentSearches.Any(s => s is null) || search is null)
                ? []
                : await _locationService.GetPossiblesAsync(parameters: [.. parentSearches]);
        }
    }
    public enum LocationProperty
    {
        Country,
        City,
        Street,
        Details
    }
}
