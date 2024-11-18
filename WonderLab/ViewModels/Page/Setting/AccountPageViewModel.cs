using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using MinecraftLaunch.Classes.Models.Auth;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WonderLab.Services.Accounts;
using WonderLab.ViewModels.Dialog;

namespace WonderLab.ViewModels.Page.Setting;

public sealed partial class AccountPageViewModel : ObservableObject {
    private readonly AccountService _accountService;

    public bool HasAccount => Accounts?.Count > 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasAccount))]
    private ReadOnlyObservableCollection<Account> _accounts;

    public AccountPageViewModel(AccountService accountService) {
        _accountService = accountService;

        Accounts = new(_accountService.Accounts);
    }

    [RelayCommand]
    private Task CreateAccount() => Dispatcher.UIThread.InvokeAsync(async () => {
        ChooseAccountTypeDialog dialog = new() {
            DataContext = App.Get<ChooseAccountTypeDialogViewModel>()
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });
}