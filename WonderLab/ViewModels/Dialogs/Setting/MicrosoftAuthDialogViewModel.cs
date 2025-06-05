using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Services.Authentication;

namespace WonderLab.ViewModels.Dialogs.Setting;

public sealed partial class MicrosoftAuthDialogViewModel : DialogViewModelBase {
    private readonly AccountService _accountService;
    private readonly AuthenticationService _authenticationService;
    private readonly ILogger<MicrosoftAuthDialogViewModel> _logger;

    private string _authLink;
    private CancellationTokenSource _tokenSource = new();

    [ObservableProperty] private string _deviceCode;
    [ObservableProperty] private bool _isDeviceCodeHas;

    public MicrosoftAuthDialogViewModel(AccountService accountService, AuthenticationService authenticationService, ILogger<MicrosoftAuthDialogViewModel> logger) {
        _logger = logger;
        _accountService = accountService;
        _authenticationService = authenticationService;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        try {
            var account = await _authenticationService.LoginMicrosoftAccountAsync(x => {
                IsDeviceCodeHas = !string.IsNullOrEmpty(x.DeviceCode);
                DeviceCode = x.UserCode;

                _authLink = x.VerificationUrl;
            }, _tokenSource.Token);

            _accountService.AddAccount(account);

            await Dispatcher.UIThread.InvokeAsync(() => CloseCommand?.Execute(null));
        } catch (OperationCanceledException) { }
    });

    [RelayCommand]
    private Task CopyCode() => Dispatcher.UIThread.InvokeAsync(async () => {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            await lifetime.MainWindow.Clipboard.SetTextAsync(DeviceCode);
    });

    [RelayCommand]
    private Task JumpLink() => Dispatcher.UIThread.InvokeAsync(async () => {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) {
            await lifetime.MainWindow.Clipboard.SetTextAsync(DeviceCode);
            await lifetime.MainWindow.Launcher.LaunchUriAsync(new(_authLink));
        }

    });

    public override void Close() {
        _tokenSource.Cancel();
        LoadedCommand.Cancel();
        base.Close();
    }
}