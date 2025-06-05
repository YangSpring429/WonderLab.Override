using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftLaunch.Base.Models.Authentication;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using WonderLab.Services;
using WonderLab.Services.Authentication;

namespace WonderLab.ViewModels.Pages.Setting;

public sealed partial class AccountPageViewModel : ObservableObject {
    private readonly DialogService _dialogService;
    private readonly AccountService _accountService;

    [ObservableProperty] private bool _hasAccounts;

    public ReadOnlyObservableCollection<Account> Accounts { get; set; }

    public AccountPageViewModel(AccountService accountService, DialogService dialogService) {
        _dialogService = dialogService;
        _accountService = accountService;

        Accounts = new(_accountService.Accounts);

        HasAccounts = Accounts.Count is > 0;
        _accountService.Accounts.CollectionChanged += OnCollectionChanged;
    }

    [RelayCommand]
    private async Task ShowAddAccountDialog() {
        await _dialogService.ShowDialogAsync("ChooseAccountType");
    }

    [RelayCommand]
    private void ActiveAccount(Account account) {
        _accountService.ActivateAccount(account);
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        HasAccounts = Accounts.Count is > 0;
    }
}