using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using System.Linq;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Accounts;

namespace WonderLab.ViewModels.Dialog.Auth;

public sealed partial class OfflineAuthDialogViewMdoel : ObservableObject {
    private readonly AccountService _accountService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string _userName;

    public OfflineAuthDialogViewMdoel(AccountService accountService) {
        _accountService = accountService;
    }

    private bool CanSave() => !string.IsNullOrEmpty(UserName);

    [RelayCommand]
    private void Close() {
        DialogHost.Close("PART_DialogHost");
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save() {
        if (_accountService.Accounts.Any(x => x.Name == UserName)) {
            WeakReferenceMessenger.Default.Send(new NotificationMessage($"已存在用户名为\"{UserName}\"的离线账户！", NotificationType.Warning));
            return;
        }

        Close();
        _accountService.Accounts.Add(_accountService.CreateOfflineAccount(UserName));
    }
}