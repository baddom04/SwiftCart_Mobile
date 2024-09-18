using Android.Content;
using ShoppingList.Utils;
using System;
using System.IO;

namespace ShoppingList.Android;

public class AndroidFileService : IFileService
{
    private readonly Context _context;
    public AndroidFileService(Context context)
    {
        if (context.FilesDir is null) throw new Exception("FileDir property is null!");
        _context = context;
    }
    public string? ReadFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return null;

        string path = Path.Combine(_context.FilesDir!.AbsolutePath, filePath);
        return File.ReadAllText(path);
    }

    public bool SaveFile(string filePath, string content)
    {
        if(string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(content)) return false;

        string path = Path.Combine(_context.FilesDir!.AbsolutePath, filePath);

        try { File.WriteAllText(path, content); } 
        catch { return false; }

        return true;
    }
}