using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class PossibleLocations
    {
        public IEnumerable<string> Countries { get; init; } = [];
        public IEnumerable<string> Cities { get; init; } = [];
        public IEnumerable<string> Streets { get; init; } = [];
        public IEnumerable<string> Details { get; init; } = [];
    }
}
