using ShoppingList.Core.Enums;

namespace ShoppingList.Core
{
    public class User : ICloneable
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public DateTime? EmailVerifiedAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public UserRole Admin { get; init; }

        public object Clone()
        {
            return new User
            {
                Id = Id,
                Name = Name,
                Email = Email,
                EmailVerifiedAt = EmailVerifiedAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Admin = Admin,
            };
        }
    }
}
