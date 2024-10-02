﻿using ShoppingList.Utils;
using System;
using System.IO;

namespace ShoppingList.Desktop
{
    public class DesktopFileService : IFileService
    {
        private readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ShoppingList");

        public DesktopFileService()
        {
            if(!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);
        }
        public string? ReadFile(string filePath)
        {
            if(string.IsNullOrWhiteSpace(filePath)) return null;

            string path = Path.Combine(_folderPath, filePath);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public bool SaveFile(string filePath, string content)
        {
            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(content)) return false;

            string path = Path.Combine(_folderPath, filePath);

            try { File.WriteAllText(path, content); }
            catch { return false; }

            return true;
        }
    }
}
