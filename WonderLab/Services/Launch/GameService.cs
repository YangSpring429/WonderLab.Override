using CommunityToolkit.Mvvm.Collections;
using Microsoft.Extensions.Logging;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Components.Parser;
using System;

namespace WonderLab.Services.Launch;

public sealed class GameService {
    private readonly ILogger<GameService> _logger;
    private readonly ConfigService _configService;
    private readonly ObservableGroupedCollection<MinecraftEntry, GameProfileEntry> _minecrafts;

    public MinecraftParser MinecraftParser;
    public ObservableGroup<MinecraftEntry, GameProfileEntry> ActiveGame { get; private set; }
    public ReadOnlyObservableGroupedCollection<MinecraftEntry, GameProfileEntry> Minecrafts { get; }

    public event EventHandler ActiveGameChanged;

    public GameService(ILogger<GameService> logger, ConfigService configService) {
        _logger = logger;
        _configService = configService;

        //if (string.IsNullOrWhiteSpace(_configService.Entries.ActiveMinecraftFolder) ||
        //    !_configService.Entries.MinecraftFolders
        //        .Contains(_configService.Entries.ActiveMinecraftFolder)) {
        //    MinecraftParser = _configService.Entries.ActiveMinecraftFolder;
        //}

        _minecrafts = [];
        //MinecraftParser ??= @".minecraft";
        //foreach (var minecraft in MinecraftParser.GetMinecrafts()) {
        //    if (MinecraftParser.LauncherProfileParser.Profiles.TryGetValue(minecraft.Id, out var profile))
        //        _minecrafts.AddItem(minecraft, profile);
        //}

        Minecrafts = new(_minecrafts);
    }

    public void RefreshGames() {
        _minecrafts.Clear();

        if (MinecraftParser is null && string.IsNullOrEmpty(_configService.Entries?.ActiveMinecraftFolder))
            throw new InvalidOperationException("The minecraft parser is not initialized.");

        MinecraftParser ??= _configService.Entries.ActiveMinecraftFolder;
        foreach (var minecraft in MinecraftParser.GetMinecrafts())
            if (MinecraftParser.LauncherProfileParser.Profiles.TryGetValue(minecraft.Id, out var profile))
                _minecrafts.AddItem(minecraft, profile);

        if (_minecrafts.Count == 0)
            return;

        ActivateMinecraft(_minecrafts[0].Key);
    }

    public void ActivateMinecraftFolder(string dir) {
        if (!_configService.Entries.MinecraftFolders.Contains(dir))
            throw new ArgumentException("The specified minecraft folder does not exist.");

        MinecraftParser = _configService.Entries.ActiveMinecraftFolder = dir;
        RefreshGames();
    }

    public void ActivateMinecraft(MinecraftEntry entry) {
        if (entry != null && !ContainsKey(entry))
            throw new ArgumentException("The specified minecraft entry does not exist.");

        ActiveGameChanged?.Invoke(this, EventArgs.Empty);
        ActiveGame = _minecrafts.FirstGroupByKey(entry);
    }

    private bool ContainsKey(MinecraftEntry minecraft) {
        foreach (var item in _minecrafts)
            if (item.Key == minecraft)
                return true;

        return false;
    }
}

//public sealed partial class GameViewModel : ObservableObject {
//    private readonly GameService _gameService;

//    public GameModel Model { get; }

//    public GameViewModel(GameModel gameModel, GameService gameService) {
//        Model = gameModel;
//        _gameService = gameService;
//    }

//    [RelayCommand]
//    private void Delete() {
//        _gameService.Save();
//    }


//    [RelayCommand]
//    private void Collect() {
//        Model.Model.IsCollection = true;
//        _gameService.Save();
//    }
//}