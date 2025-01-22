using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MinecraftLaunch.Classes.Models.Auth;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Accounts;

namespace WonderLab.ViewModels.Page.GameSetting;

public sealed partial class ChooseAccountPageViewModel : ObservableObject {
    private readonly AccountService _accountService;

    [ObservableProperty] private bool _hasAccount = true;

    [ObservableProperty] private ReadOnlyObservableCollection<Account> _accounts;

    public ChooseAccountPageViewModel(AccountService accountService) {
        _accountService = accountService;
        _accountService.CollectionChanged += OnCollectionChanged;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        await Task.Delay(TimeSpan.FromSeconds(0.45));
        Accounts = _accountService.Accounts;
        HasAccount = Accounts?.Count > 0;
    });

    [RelayCommand]
    private void ClosePanel() {
        WeakReferenceMessenger.Default.Send(new PanelPageNotificationMessage("close"));
    }

    private void OnCollectionChanged(object sender, EventArgs e) {
        HasAccount = Accounts?.Count > 0;
    }
}