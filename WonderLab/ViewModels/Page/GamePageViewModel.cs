using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MinecraftLaunch.Base.Models.Game;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Page;

public sealed partial class GamePageViewModel : ObservableObject {
    private readonly GameService _gameService;

    [ObservableProperty] private ObservableGroup<MinecraftEntry, GameProfileEntry> _activeGame;
    [ObservableProperty] private ReadOnlyObservableGroupedCollection<MinecraftEntry, GameProfileEntry> _games;

    public string MinecraftFolderPath => _gameService.MinecraftParser?.Root.FullName ?? "Not Found";

    public GamePageViewModel(GameService gameService) {
        _gameService = gameService;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(() => {
        Games = _gameService.Minecrafts;
        ActiveGame = _gameService.ActiveGame;
    });

    [RelayCommand]
    private void Delete() {

    }

    [RelayCommand]
    private void SelectGame(MinecraftEntry minecraft) {
        _gameService.ActivateMinecraft(minecraft);
        WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("Home"));
    }

    [RelayCommand]
    private void GoToGameSetting() {
        WeakReferenceMessenger.Default.Send<PanelPageNotificationMessage>(new("GameSetting/Navigation"));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(ActiveGame)) {
            _gameService.ActivateMinecraft(ActiveGame.Key);
        }
    }
}