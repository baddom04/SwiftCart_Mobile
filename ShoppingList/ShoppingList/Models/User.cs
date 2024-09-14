namespace ShoppingList.Models
{
    public class User(string userName, string nickName, string email)
    {
        public string UserName { get; set; } = userName;
        public string NickName { get; set; } = userName;
        public string Email { get; set; } = email;
    }
}
