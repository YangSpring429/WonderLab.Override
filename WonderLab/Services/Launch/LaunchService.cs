using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using MinecraftLaunch.Classes.Models.Launch;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Components.Launcher;
using WonderLab.Infrastructure.Models;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Services.Account;
using WonderLab.Services.UI;
using WonderLab.ViewModels.Tasks;

namespace WonderLab.Services.Launch;

public sealed class LaunchService {
    private readonly TaskService _taskService;
    private readonly GameService _gameService;
    private readonly ConfigService _configService;
    private readonly AccountService _accountService;
    private readonly NotificationService _notificationService;

    public LaunchService(
        GameService gameService,
        TaskService taskService,
        AccountService accountService,
        ConfigService configService,
        NotificationService notificationService) {
        _taskService = taskService;
        _gameService = gameService;
        _configService = configService;
        _accountService = accountService;
        _notificationService = notificationService;
    }

    public async Task LaunchTaskAsync(GameModel game) {
        LaunchTaskViewModel task = new() {
            JobName = $"游戏 {game.Entry.Id} 的启动任务"
        };
        
        _taskService.QueueJob(task);
        await Task.Run(async () => await LaunchAsync(_configService.Entries.ActiveAccount, task, task.TaskCancellationToken));
    }

    private async Task LaunchAsync(
        MinecraftLaunch.Classes.Models.Auth.Account account,
        IProgress<TaskProgress> progress,
        CancellationToken cancellationToken) {
                double progressCache = 0d;
        DispatcherTimer dispatcherTimer = new(DispatcherPriority.ApplicationIdle) {
            Interval = TimeSpan.FromSeconds(0.2d),
        };

        dispatcherTimer.Tick += (_, _) => {
            progress.Report(new(3, progressCache));
        };

        try {
            var config = _configService.Entries;

            progress.Report(new(1, 0d));

            if (config.ActiveJava is null) {
                throw new ArgumentException("未选择任何 Java 实例");
            }

            var javaPath = config.IsAutoSelectJava ? config.ActiveJava : config.ActiveJava!;
            progress.Report(new(1, 1d));
            cancellationToken.ThrowIfCancellationRequested();

            //Auth
            progress.Report(new(2, 0d));

            //await RefreshAccountAsync(account);

            progress.Report(new(2, 1d));
            cancellationToken.ThrowIfCancellationRequested();

            //Complete
            progress.Report(new(3, 0d));

            //await ResolveAndDownloadResourceAsync(x => progressCache = x, cancellationToken);

            progress.Report(new(3, 1d));
            cancellationToken.ThrowIfCancellationRequested();

            //Launch
            progress.Report(new(4, 0d));

            Launcher launcher = new(_gameService.GameResolver, new() {
                JvmConfig = new JvmConfig(javaPath.JavaPath) {
                    MaxMemory = 1024
                },
                Account = account ?? new OfflineAuthenticator("Steve").Authenticate(),
                IsEnableIndependencyCore = config.IsGameIndependent,
                LauncherName = "WonderLab"
            });

            await launcher.LaunchAsync(_gameService.ActiveGame.Entry.Id);

            progress.Report(new(4, 1d));
        } catch (Exception ex) {
            progress.Report(new(-1, 1d, ex));
        }
    }
}