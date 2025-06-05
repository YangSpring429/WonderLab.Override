using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using WonderLab.Services.Authentication;

namespace WonderLab.ViewModels.Dialogs.Setting;

public sealed partial class YggdrasilAuthDialogViewModel : DialogViewModelBase {
    private readonly AccountService _accountService;
    private readonly AuthenticationService _authenticationService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginAccountCommand))]
    private string _playerEmail = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginAccountCommand))]
    private string _playerPassword = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginAccountCommand))]
    private string _yggdrasilUrl = "";

    public YggdrasilAuthDialogViewModel(AccountService accountService, AuthenticationService authenticationService) {
        _accountService = accountService;
        _authenticationService = authenticationService;
    }

    [RelayCommand(CanExecute = nameof(CanLoginAccount))]
    private async Task LoginAccount() {
        var accounts = await _authenticationService
            .LoginYggdrasilAccountsAsync(PlayerEmail, PlayerPassword, YggdrasilUrl);

        foreach (var account in accounts)
            _accountService.AddAccount(account);

        CloseCommand?.Execute(null);
    }

    private bool CanLoginAccount() =>
        !string.IsNullOrEmpty(PlayerEmail) &&
        !string.IsNullOrEmpty(PlayerPassword) &&
        !string.IsNullOrEmpty(YggdrasilUrl);
}