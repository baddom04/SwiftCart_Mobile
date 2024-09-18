namespace ShoppingList.Utils
{
    public interface IFileService
    {
        bool SaveFile(string filePath, string content);
        string? ReadFile(string filePath);
    }
}
