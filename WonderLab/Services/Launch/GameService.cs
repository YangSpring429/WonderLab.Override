using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Components.Parser;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace WonderLab.Services.Launch;

public sealed class GameService {
    private readonly ILogger<GameService> _logger;
    private readonly ConfigService _configService;
    private readonly ObservableCollection<MinecraftEntry> _minecrafts;

    public MinecraftParser MinecraftParser { get; set; }
    public MinecraftEntry ActiveGame { get; private set; }
    public ReadOnlyObservableCollection<MinecraftEntry> Minecrafts { get; }

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
            _minecrafts.Add(minecraft);

        if (_minecrafts.Count == 0)
            return;

        var entry = _configService.Entries.ActiveGameId is not null
            ? _minecrafts.FirstOrDefault(x => x.Id == _configService.Entries.ActiveGameId)
            : _minecrafts.FirstOrDefault();

        ActivateMinecraft(entry);
    }

    public void ActivateMinecraftFolder(string dir) {
        if (!_configService.Entries.MinecraftFolders.Contains(dir))
            throw new ArgumentException("The specified minecraft folder does not exist.");

        MinecraftParser = _configService.Entries.ActiveMinecraftFolder = dir;
        RefreshGames();
    }

    public void ActivateMinecraft(MinecraftEntry entry) {
        if (entry != null && !_minecrafts.Contains(entry))
            throw new ArgumentException("The specified minecraft entry does not exist.");

        WeakReferenceMessenger.Default.Send(new ActiveMinecraftChangedMessage());
        ActiveGame = entry;
    }
}

internal record ActiveMinecraftChangedMessage;