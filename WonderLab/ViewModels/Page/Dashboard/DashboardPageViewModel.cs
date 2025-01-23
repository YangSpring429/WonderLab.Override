using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MinecraftLaunch.Classes.Models.Auth;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Accounts;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Page.Dashboard;

public sealed partial class DashboardPageViewModel : ObservableObject {
    private readonly GameService _gameService;
    private readonly AccountService _accountService;

    [ObservableProperty] private GameModel _activeGame;
    [ObservableProperty] private ReadOnlyObservableCollection<GameModel> _games;
    [ObservableProperty] private ReadOnlyObservableCollection<Account> _accounts;

    public DashboardPageViewModel(AccountService accountService, GameService gameService) {
        _gameService = gameService;
        _accountService = accountService;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        await Task.Delay(TimeSpan.FromSeconds(0.45));
        Games = _gameService.Games;
        Accounts = _accountService.Accounts;
        ActiveGame = _gameService.ActiveGame;
    });

    [RelayCommand]
    private void ClosePanel() {
        WeakReferenceMessenger.Default.Send(new PanelPageNotificationMessage("close"));
    }

    partial void OnActiveGameChanged(GameModel value) {
        _gameService.ActivateGame(value);
    }
}