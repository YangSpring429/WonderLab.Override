using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Accounts;

namespace WonderLab.ViewModels.Dialog.Auth;

public sealed partial class MicrosoftAuthDialogViewModel : ObservableObject {
    private readonly AccountService _accountService;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    [ObservableProperty] private string _verificationUrl;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCodeLoaded))]
    private string _userCode;

    public bool IsCodeLoaded => !string.IsNullOrEmpty(UserCode);

    public MicrosoftAuthDialogViewModel(AccountService accountService) {
        _accountService = accountService;
    }

    private bool CanCopy() => !string.IsNullOrEmpty(UserCode);

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        var account = await _accountService.CreateMicrosoftAccount(x => {
            UserCode = x.UserCode;
            VerificationUrl = x.VerificationUrl;
        }, _cancellationTokenSource);

        Close();
        WeakReferenceMessenger.Default.Send(new NotificationMessage($"已将微软账户\"{account.Name}\"添加至 WonderLab！", NotificationType.Warning));
    });

    [RelayCommand]
    private void Close() => Dispatcher.UIThread.InvokeAsync(() => {
        DialogHost.Close("PART_DialogHost");

        try {
            _cancellationTokenSource?.Cancel();
        } catch (System.Exception) {}
    });

    [RelayCommand]
    private void OpenUrl() => Task.Run(() => {
        Process.Start(new ProcessStartInfo(VerificationUrl) {
            UseShellExecute = true,
            Verb = "open"
        }).Dispose();
    });

    [RelayCommand]
    private void CopyCode() => Dispatcher.UIThread.InvokeAsync(async () => {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            var clipboard = desktop.MainWindow?.Clipboard;

            if (clipboard != null) {
                await clipboard.SetTextAsync(UserCode);
            }
        }
    });
}