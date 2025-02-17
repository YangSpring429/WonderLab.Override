using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using MinecraftLaunch.Base.Models.Game;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Page;

public sealed partial class HomePageViewModel : ObservableObject {
    private static bool _registerLock = false;

    private readonly GameService _gameService;
    private readonly LaunchService _launchService;
    private readonly ConfigService _configService;
    private readonly ILogger<HomePageViewModel> _logger;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LaunchCommand))]
    private MinecraftEntry _activeGame;
    public HomePageViewModel(GameService gameService, LaunchService launchService, ConfigService configService, ILogger<HomePageViewModel> logger) {
        _logger = logger;
        _gameService = gameService;
        _configService = configService;
        _launchService = launchService;

        if (!_registerLock) {
            _registerLock = true;
            WeakReferenceMessenger.Default.Register<ActiveMinecraftChangedMessage>(this, (_, _) => {
                ActiveGame = _gameService.ActiveGame;
            });
        }
    }

    private bool CanLaunch() => ActiveGame is not null;

    [RelayCommand]
    private void OnLoaded() {
        ActiveGame = _gameService.ActiveGame;
    }

    [RelayCommand(CanExecute = nameof(CanLaunch))]
    private Task Launch() => Task.Run(async () => {
        var text = I18NExtension.Translate(LanguageKeys.Launch_Notification);

        WeakReferenceMessenger.Default.Send(new NotificationMessage(string.Format(text, ActiveGame.Id),
            NotificationType.Information, () => {
                WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("TaskList"));
            }));

        await _launchService.LaunchTaskAsync(ActiveGame);
    });

    [RelayCommand]
    private void NavigationToGame() {
        WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("Game"));
    }
}