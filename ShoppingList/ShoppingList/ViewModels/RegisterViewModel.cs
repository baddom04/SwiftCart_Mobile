﻿using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ShoppingList.Model.Models;
using ShoppingList.Persistor;
using ShoppingList.Persistor.Services.Interfaces;
using ShoppingList.Utils;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> LoginPageCommand { get; }
        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }
        public string UsernameInput { get; set; } = string.Empty;
        public string EmailInput { get; set; } = string.Empty;
        public string PasswordInput { get; set; } = string.Empty;

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        private readonly Action<bool> _showLoading;
        private readonly Action<Page> _changePage;

        private readonly UserAccountModel _model;

        public RegisterViewModel(UserAccountModel model, Action<Page> changePage, Action<bool> showLoading)
        {
            _changePage = changePage;
            _showLoading = showLoading;
            _model = model;

            LoginPageCommand = ReactiveCommand.Create(() => _changePage(Page.Login));
            RegisterCommand = ReactiveCommand.CreateFromTask(Register);
        }

        private async Task Register()
        {
            if (!Validate()) return;

            _showLoading(true);

            try
            {
                IUserService userService = AppServiceProvider.Services.GetRequiredService<IUserService>();
                await userService.RegisterAsync(UsernameInput, EmailInput, PasswordInput);

                _changePage(Page.Main);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{StringProvider.GetString("RegisterError")}{ex.Message}";
            }
            finally
            {
                _showLoading(false);
            }
        }

        private bool Validate()
        {
            if (ValidateUsername() && ValidateEmail() && ValidatePassword())
            {
                ErrorMessage = null;
                return true;
            }
            return false;
        }

        private bool ValidateUsername()
        {
            string trimmedUsername = UsernameInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedUsername))
            {
                ErrorMessage = StringProvider.GetString("UsernameMissingError");
                return false;
            }

            return true;
        }

        private bool ValidatePassword()
        {
            string trimmedPassword = PasswordInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedPassword))
            {
                ErrorMessage = StringProvider.GetString("PasswordMissingError");
                return false;
            }

            if (trimmedPassword.Length < 8)
            {
                ErrorMessage = StringProvider.GetString("PasswordFormatError");
                return false;
            }

            return true;
        }

        private bool ValidateEmail()
        {
            var trimmedEmail = EmailInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmedEmail))
            {
                ErrorMessage = StringProvider.GetString("EmailMissingError");
                return false;
            }

            if (trimmedEmail.EndsWith('.'))
            {
                ErrorMessage = StringProvider.GetString("EmailFormatError");
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(EmailInput);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                ErrorMessage = StringProvider.GetString("EmailFormatError");
                return false;
            }
        }
    }
}
