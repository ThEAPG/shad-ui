﻿using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI.Dialogs;
using ShadUI.Toasts;

namespace ShadUI.Demo.ViewModels;

public sealed partial class DialogsViewModel : ViewModelBase
{
    private readonly DialogManager _dialogManager;
    private readonly ToastManager _toastManager;
    private readonly LoginViewModel _loginViewModel;

    public DialogsViewModel(DialogManager dialogManager,
        ToastManager toastManager,
        LoginViewModel loginViewModel)
    {
        _dialogManager = dialogManager;
        _toastManager = toastManager;
        _loginViewModel = loginViewModel;

        var path = Path.Combine(AppContext.BaseDirectory, "viewModels", "DialogsViewModel.cs");
        AlertDialogCode = WrapCode(path.ExtractByLineRange(46, 62).CleanIndentation());
        DestructiveAlertDialogCode = WrapCode(path.ExtractByLineRange(67, 84).CleanIndentation());
        CustomDialogCode = WrapCode(path.ExtractByLineRange(89, 106).CleanIndentation());
    }

    private string WrapCode(string code)
    {
        return $"""
                using CommunityToolkit.Mvvm.Input;

                //..other code

                {code}

                //..rest of the code
                """;
    }

    [ObservableProperty]
    private string _alertDialogCode = string.Empty;

    [RelayCommand]
    private void ShowDialog()
    {
        _dialogManager
            .CreateDialog(
                "Are you absolutely sure?",
                "This action cannot be undone. This will permanently delete your account and remove your data from our servers.")
            .WithPrimaryButton("Continue",
                () => _toastManager.CreateToast("Delete account")
                    .WithContent("Account deleted successfully!")
                    .DismissOnClick()
                    .ShowSuccess())
            .WithCancelButton("Cancel")
            .WithMaxWidth(512)
            .Dismissible()
            .Show();
    }

    [ObservableProperty]
    private string _destructiveAlertDialogCode = string.Empty;

    [RelayCommand]
    private void ShowDestructiveStyleDialog()
    {
        _dialogManager
            .CreateDialog(
                "Are you absolutely sure?",
                "This action cannot be undone. This will permanently delete your account and remove your data from our servers.")
            .WithPrimaryButton("Continue",
                () => _toastManager.CreateToast("Delete account")
                    .WithContent("Account deleted successfully!")
                    .DismissOnClick()
                    .ShowSuccess()
                , DialogButtonStyle.Destructive)
            .WithCancelButton("Cancel")
            .WithMaxWidth(512)
            .Dismissible()
            .Show();
    }

    [ObservableProperty]
    private string _customDialogCode = string.Empty;

    [RelayCommand]
    private void ShowCustomDialog()
    {
        _loginViewModel.Initialize();
        _dialogManager.CreateDialog(_loginViewModel)
            .Dismissible()
            .WithSuccessCallback(() =>
                _toastManager.CreateToast("Sign in successful")
                    .WithContent("Welcome back!")
                    .DismissOnClick()
                    .ShowSuccess())
            .WithCancelCallback(() =>
                _toastManager.CreateToast("Sign in cancelled")
                    .WithContent("Please sign in to continue.")
                    .DismissOnClick()
                    .ShowWarning())
            .Show();
    }
}