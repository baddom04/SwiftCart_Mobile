namespace ShoppingList.Models
{
    public class User(string userName, string nickName, string email)
    {
        public string UserName { get; set; } = userName;
        public string NickName { get; set; } = nickName;
        public string Email { get; set; } = email;

        public override bool Equals(object? obj)
        {
            if(obj is null || obj is not User other) return false;
            if(obj == this) return true;

            return other.UserName == UserName && other.NickName == NickName && other.Email == Email;
        }
    }
}
