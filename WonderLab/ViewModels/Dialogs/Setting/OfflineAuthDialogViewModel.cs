using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using WonderLab.Services.Authentication;

namespace WonderLab.ViewModels.Dialogs.Setting;

public sealed partial class OfflineAuthDialogViewModel : DialogViewModelBase {
    private readonly AccountService _accountService;
    private readonly AuthenticationService _authenticationService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CreateAccountCommand))]
    private string _playerName = "";

    public OfflineAuthDialogViewModel(AccountService accountService, AuthenticationService authenticationService) {
        _accountService = accountService;
        _authenticationService = authenticationService;
    }

    [RelayCommand(CanExecute = nameof(CanCreateAccount))]
    private void CreateAccount() {
        var account = _authenticationService.CreateOfflineAccount(PlayerName);
        _accountService.AddAccount(account);

        CloseCommand?.Execute(null);
    }

    [GeneratedRegex("^[a-zA-Z0-9_]+$")]
    private partial Regex PlayerNameRegex();

    private bool CanCreateAccount() => (PlayerName.Length > 3 && PlayerName.Length < 16) && PlayerNameRegex().IsMatch(PlayerName);
}