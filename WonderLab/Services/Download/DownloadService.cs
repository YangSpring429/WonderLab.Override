using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Messaging;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;
using System;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Models;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Launch;
using WonderLab.ViewModels.Tasks;

namespace WonderLab.Services.Download;

public sealed class DownloadService {
    private readonly TaskService _taskService;
    private readonly GameService _gameService;
    private readonly ConfigService _configService;

    public DownloadService(ConfigService configService, GameService gameService, TaskService taskService) {
        _taskService = taskService;
        _gameService = gameService;
        _configService = configService;
    }

    public Task InstallMinecraftTaskAsync(string gameId, bool isInstallOptifine = false, object installEntry = default, string customGameId = default) {
        var id = string.IsNullOrEmpty(customGameId) ? gameId : customGameId;
        InstallMinecraftTaskViewModel task = new() {
            JobName = $"游戏实例 {id} 的安装任务"
        };

        task.Completed += (_, _) => {
            WeakReferenceMessenger.Default.Send(new NotificationMessage($"游戏实例 {id} 安装完成！", NotificationType.Information));

            _gameService.RefreshGames();
        };

        _taskService.QueueJob(task);
        return Task.Run(async () => await InstallMinecraftAsync(gameId, task, task.TaskCancellationToken, customGameId, installEntry, isInstallOptifine));
    }

    public async Task InstallMinecraftAsync(
        string gameId,
        IProgress<TaskProgress> progress,
        CancellationToken cancellationToken,
        string customGameId = default,
        object installEntry = default,
        bool isInstallOptifine = false) {
        var dc = new DownloaderConfiguration {
            IsEnableFragmentedDownload = true,
            MaxThread = _configService.Entries.ThreadCount
        };

        try {
            var mainInstaller = new VanlliaInstaller(_gameService.GameResolver, gameId, dc);
            InstallerBase installer = installEntry is null
                ? mainInstaller
                : installEntry switch {
                    ForgeInstallEntry => new CompositionInstaller(mainInstaller, new ForgeInstaller((ForgeInstallEntry)installEntry, _configService.Entries.ActiveJava.JavaPath, customGameId, dc), customGameId),
                    FabricBuildEntry => new CompositionInstaller(mainInstaller, new FabricInstaller((FabricBuildEntry)installEntry, customGameId, dc), customGameId),
                    QuiltBuildEntry => new CompositionInstaller(mainInstaller, new QuiltInstaller((QuiltBuildEntry)installEntry, customGameId, dc), customGameId),
                    _ => throw new NotSupportedException()
                };

            progress.Report(new(1, 1d));
            cancellationToken.ThrowIfCancellationRequested();

            if (installer is CompositionInstaller composition) {
                bool isSub1Installed = false;
                composition.SubInstallerCompleted += (_, _) => {
                    progress.Report(new(2, 1d));
                    isSub1Installed = true;
                };

                composition.ProgressChanged += (_, arg) => {
                    if (isSub1Installed) {
                        progress.Report(new(3, arg.Progress, Speed: arg.Speed));
                    } else {
                        progress.Report(new(2, arg.Progress, Speed: arg.Speed));
                    }
                };

                composition.Completed += (_, arg) => progress.Report(new(4, 1d));
            } else {
                installer.ProgressChanged += (_, arg) => {
                    progress.Report(new(2, arg.Progress, Speed: arg.Speed));
                };

                installer.Completed += (_, arg) => {
                    progress.Report(new(3, 1d));
                    progress.Report(new(4, 1d));
                };
            }

            await installer.InstallAsync(cancellationToken);
        } catch (Exception ex) {
            progress.Report(new(-1, 1d, ex));
        }
    }
}