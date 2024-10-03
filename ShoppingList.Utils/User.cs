namespace ShoppingList.Utils
{
    public class User(string userName, string nickName, string email) : ICloneable
    {
        public string UserName { get; set; } = userName;
        public string NickName { get; set; } = nickName;
        public string Email { get; set; } = email;

        public static User Empty => new(string.Empty, string.Empty, string.Empty);
        public object Clone()
        {
            return new User(UserName, NickName, Email);
        }
        public override bool Equals(object? obj)
        {
            if(obj is null || obj is not User other) return false;
            if(obj == this) return true;

            return other.UserName == UserName && other.NickName == NickName && other.Email == Email;
        }
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
