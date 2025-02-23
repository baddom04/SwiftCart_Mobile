using ShoppingList.Persistor.Services.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Desktop
{
    internal class DesktopTokenService : ITokenService
    {
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SwiftCart", "token.dat");
        public Task ClearTokenAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                    File.Delete(_filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to remove the token securely.", ex);
            }

            return Task.CompletedTask;
        }

        public async Task<string?> GetTokenAsync()
        {
            if (!OperatingSystem.IsWindows()) return null;

            try
            {
                if (!File.Exists(_filePath))
                    return null;

                byte[] encryptedBytes = await File.ReadAllBytesAsync(_filePath);
                byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve the token securely.", ex);
            }
        }

        public async Task SaveTokenAsync(string token)
        {
            if (!OperatingSystem.IsWindows()) return;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

                byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                byte[] encryptedBytes = ProtectedData.Protect(tokenBytes, null, DataProtectionScope.CurrentUser);

                await File.WriteAllBytesAsync(_filePath, encryptedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save the token securely.", ex);
            }
        }
    }
}
