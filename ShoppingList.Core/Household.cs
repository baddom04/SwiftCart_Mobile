using ShoppingList.Core.Enums;

namespace ShoppingList.Core
{
    public class Household
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Identifier { get; init; }
        public int UserId { get; init; }
        public HouseholdRelationship? Relationship { get; set; }
    }
}
