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
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Page;

public sealed partial class HomePageViewModel : ObservableObject {
    private readonly GameService _gameService;
    private readonly LaunchService _launchService;
    private readonly ConfigService _configService;
    private readonly ILogger<HomePageViewModel> _logger;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LaunchCommand))]
    private ObservableGroup<MinecraftEntry, GameProfileEntry> _activeGame;

    public HomePageViewModel(GameService gameService, LaunchService launchService, ConfigService configService, ILogger<HomePageViewModel> logger) {
        _logger = logger;
        _gameService = gameService;
        _configService = configService;
        _launchService = launchService;

        _gameService.ActiveGameChanged += OnActiveGameChanged;
        _gameService.RefreshGames();
    }

    private bool CanLaunch() => ActiveGame is not null;

    [RelayCommand(CanExecute = nameof(CanLaunch))]
    private Task Launch() => Task.Run(async () => {
        var text = I18NExtension.Translate(LanguageKeys.Launch_Notification);

        WeakReferenceMessenger.Default.Send(new NotificationMessage(string.Format(text, ActiveGame.Key.Id),
            NotificationType.Information, () => {
                WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("TaskList"));
            }));

        await _launchService.LaunchTaskAsync(ActiveGame.Key);
    });

    [RelayCommand]
    private void NavigationToGame() {
        WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("Game"));
    }

    private void OnActiveGameChanged(object sender, EventArgs e) => Dispatcher.UIThread.InvokeAsync(() => {
        ActiveGame = _gameService.ActiveGame;
    });
}