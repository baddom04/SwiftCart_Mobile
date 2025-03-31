using ShoppingList.Core;

namespace ShoppingListEditor.Model.Editables
{
    public class LocationEditable
    {
        public int Id { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string Detail { get; set; }
        public int StoreId { get; set; }

        public static LocationEditable? FromLocation(Location? location)
        {
            if(location == null) return null;

            return new LocationEditable()
            {
                Id = location.Id,
                Country = location.Country,
                ZipCode = location.ZipCode,
                City = location.City,
                Street = location.Street,
                Detail = location.Detail,
                StoreId = location.StoreId
            };
        }
    }
}
