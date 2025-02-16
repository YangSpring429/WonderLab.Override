using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MinecraftLaunch.Classes.Models.Auth;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services;
using WonderLab.Services.Accounts;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Page.Dashboard;

public sealed partial class DashboardPageViewModel : ObservableObject {
    private readonly GameService _gameService;
    private readonly ConfigService _configService;
    private readonly AccountService _accountService;

    [ObservableProperty] private GameModel _activeGame;
    [ObservableProperty] private Account _activeAccount;
    [ObservableProperty] private ReadOnlyObservableCollection<GameModel> _games;
    [ObservableProperty] private ReadOnlyObservableCollection<Account> _accounts;

    public DashboardPageViewModel(AccountService accountService, GameService gameService, ConfigService configService) {
        _gameService = gameService;
        _configService = configService;
        _accountService = accountService;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        await Task.Delay(TimeSpan.FromSeconds(0.45));
        Games = _gameService.Games;
        Accounts = _accountService.Accounts;
        ActiveAccount = Accounts.FirstOrDefault(x => _configService.Entries.ActiveAccount?.Uuid == x.Uuid);
        ActiveGame = Games.FirstOrDefault(x => _gameService.ActiveGame.Entry.Id == x.Entry.Id);
    });

    [RelayCommand]
    private void ClosePanel() {
        WeakReferenceMessenger.Default.Send(new PanelPageNotificationMessage("close"));
    }

    partial void OnActiveGameChanged(GameModel value) {
        _gameService.ActivateGame(value);
    }

    partial void OnActiveAccountChanged(Account value) {
        _configService.Entries.ActiveAccount = value;
    }
}