namespace ShoppingList.Utils
{
    public class Comment(User user, string text)
    {
        public User User { get; set; } = user;
        public string Text { get; set; } = text;
    }
}
