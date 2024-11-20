using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using System.Linq;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Accounts;

namespace WonderLab.ViewModels.Dialog.Auth;

public sealed partial class YggdrasilAuthDialogViewModel : ObservableObject {
    private readonly AccountService _accountService;

    [ObservableProperty] private bool _isUseLittleSkin;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _email;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _password;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _yggdrasilServerUrl;

    public YggdrasilAuthDialogViewModel(AccountService accountService) {
        _accountService = accountService;
    }

    private bool CanSave() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(YggdrasilServerUrl);

    partial void OnIsUseLittleSkinChanged(bool value) {
        YggdrasilServerUrl = value ? "https://littleskin.cn/api/yggdrasil" : string.Empty;
    }

    [RelayCommand]
    private void Close() {
        DialogHost.Close("PART_DialogHost");
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save() {
        var accounts = _accountService.CreateYggdrasilAccounts(Email, Password, YggdrasilServerUrl)
            .ToList();

        Close();

        if (accounts is { Count: 0 }) {
            WeakReferenceMessenger.Default.Send(new NotificationMessage($"已存在用户名为的离线账户！", NotificationType.Warning));
            return;
        }
    }
}