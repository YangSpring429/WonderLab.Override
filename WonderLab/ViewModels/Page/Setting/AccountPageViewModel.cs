using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MinecraftLaunch.Base.Models.Authentication;
using WonderLab.Services.Account;
using WonderLab.ViewModels.Dialog;

namespace WonderLab.ViewModels.Page.Setting;

public sealed partial class AccountPageViewModel : ObservableObject {
    private readonly AccountService _accountService;

    [ObservableProperty] private bool _hasAccount;

    [ObservableProperty]
    private ReadOnlyObservableCollection<Account> _accounts;

    public AccountPageViewModel(AccountService accountService) {
        _accountService = accountService;
        _accountService.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, EventArgs e) {
        HasAccount = Accounts?.Count > 0;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(() => {
        Accounts = _accountService.Accounts;
        HasAccount = Accounts?.Count > 0;
    });

    [RelayCommand]
    private Task CreateAccount() => Dispatcher.UIThread.InvokeAsync(async () => {
        ChooseAccountTypeDialog dialog = new() {
            DataContext = App.Get<ChooseAccountTypeDialogViewModel>()
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });
}