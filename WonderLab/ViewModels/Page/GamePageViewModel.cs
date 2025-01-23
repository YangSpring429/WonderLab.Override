using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAllListVisible))]
    private ReadOnlyObservableCollection<GameModel> _games;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCollectionGamesVisible))]
    private ObservableCollection<GameModel> _collectionGames;

    [ObservableProperty] private GameModel _activeGame;

    public bool IsAllListVisible => Games.Count > 0;
    public bool IsCollectionGamesVisible => CollectionGames?.Count > 0;
    public string MinecraftFolderPath => _gameService.GameResolver?.Root.FullName ?? "Not Found";

    public GamePageViewModel(GameService gameService) {
        _gameService = gameService;
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(() => {
        Games = _gameService.Games;
        ActiveGame = _gameService.ActiveGame;
        CollectionGames = Games.Where(x => x.Model.IsCollection).ToObservableList();
    });

    [RelayCommand]
    private void Collect(GameModel gameModel) {
        gameModel.Model.IsCollection = true;
        _gameService.Save();
        _gameService.RefreshGames();
        LoadedCommand.Execute(default);
    }

    [RelayCommand]
    private void Uncollect(GameModel gameModel) {
        gameModel.Model.IsCollection = false;
        _gameService.Save();
        _gameService.RefreshGames();
        LoadedCommand.Execute(default);
    }

    [RelayCommand]
    private void Delete() {

    }

    [RelayCommand]
    private void SelectGame(GameModel gameModel) {
        _gameService.ActivateGame(gameModel);
        WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("Home"));
    }

    [RelayCommand]
    private void GoToGameSetting() {
        WeakReferenceMessenger.Default.Send<PanelPageNotificationMessage>(new("GameSetting/Navigation"));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(ActiveGame)) {
            _gameService.ActivateGame(ActiveGame);
        }
    }
}