using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using MinecraftLaunch.Components.Installer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Download;

namespace WonderLab.ViewModels.Dialog.Download;

public sealed partial class InstallMinecraftDialogViewModel : ObservableObject {
    private readonly DownloadService _downloadService;

    private ModLoaderType _loaderType;

    public string GameCoreId { get; set; }

    public bool IsForgeLoaded => Forges is not null && Forges.Count > 0;
    public bool IsQuiltLoaded => Quilts is not null && Quilts.Count > 0;
    public bool IsFabricLoaded => Fabrics is not null && Fabrics.Count > 0;
    public bool IsNeoforgeLoaded => Neoforges is not null && Neoforges.Count > 0;
    public bool IsOptifineLoaded => Optifines is not null && Optifines.Count > 0;

    [ObservableProperty] private bool _isInstallOptifine;
    [ObservableProperty] private object _currentModLoader;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InstallCommand))]
    private string _customGameCoreId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsQuiltLoaded))]
    private ObservableCollection<object> _quilts;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsForgeLoaded))]
    private ObservableCollection<object> _forges;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFabricLoaded))]
    private ObservableCollection<object> _fabrics;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOptifineLoaded))]
    private ObservableCollection<object> _optifines;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNeoforgeLoaded))]
    private ObservableCollection<object> _neoforges;

    public InstallMinecraftDialogViewModel(DownloadService downloadService) {
        _downloadService = downloadService;
    }

    private bool CanInstall() => !string.IsNullOrEmpty(CustomGameCoreId);

    [RelayCommand]
    private async Task OnLoaded() {
        CustomGameCoreId = GameCoreId;
        await LoadModLoaders();
    }

    [RelayCommand]
    private Task Close() => Dispatcher.UIThread.InvokeAsync(async () => {
        DialogHost.Close("PART_DialogHost");
    });

    [RelayCommand(CanExecute = nameof(CanInstall))]
    private Task Install() => Task.Run(() => {
        //var text = I18NExtension.Translate(LanguageKeys.Launch_Notification);

        WeakReferenceMessenger.Default.Send(new NotificationMessage($"正在安装游戏实例 {CustomGameCoreId}，点击此通知以查看详情",
            NotificationType.Information, () => {
                WeakReferenceMessenger.Default.Send<PageNotificationMessage>(new("TaskList"));
            }));

        _downloadService.InstallMinecraftTaskAsync(GameCoreId, IsInstallOptifine, CurrentModLoader, CustomGameCoreId);
        Close();
    });

    [RelayCommand]
    private void SelectedModLoader(string type) {
        _loaderType = type switch {
            "Not" => ModLoaderType.Any,
            "Forge" => ModLoaderType.Forge,
            "Quilt" => ModLoaderType.Quilt,
            "Fabric" => ModLoaderType.Fabric,
            "Neoforge" => ModLoaderType.NeoForge,
            _ => ModLoaderType.Any,
        };
    }

    private Task LoadModLoaders() {
        var tasks = new List<Task>() {
            Task.Run(() => LoadModLoader(ModLoaderType.Quilt)),
            Task.Run(() => LoadModLoader(ModLoaderType.Forge)),
            Task.Run(() => LoadModLoader(ModLoaderType.Fabric)),
            Task.Run(() => LoadModLoader(ModLoaderType.NeoForge)),
            Task.Run(() => LoadModLoader(ModLoaderType.OptiFine)),
        };

        return Task.WhenAll(tasks);

        async void LoadModLoader(ModLoaderType type) {
            IEnumerable<object> loaders = type switch {
                ModLoaderType.Forge => await ForgeInstaller.EnumerableForgeAsync(GameCoreId).ToListAsync(),
                ModLoaderType.Quilt => await QuiltInstaller.EnumerableQuiltAsync(GameCoreId).ToListAsync(),
                ModLoaderType.Fabric => await FabricInstaller.EnumerableFabricAsync(GameCoreId).ToListAsync(),
                ModLoaderType.OptiFine => await OptifineInstaller.EnumerableOptifineAsync(GameCoreId).ToListAsync(),
                _ => null
            };

            Dispatcher.UIThread.Post(() => {
                switch (type) {
                    case ModLoaderType.Forge:
                        Forges = loaders.ToObservableList();
                        break;
                    case ModLoaderType.Quilt:
                        Quilts = loaders.ToObservableList();
                        break;
                    case ModLoaderType.Fabric:
                        Fabrics = loaders.ToObservableList();
                        break;
                    case ModLoaderType.OptiFine:
                        Optifines = loaders.ToObservableList();
                        break;
                    case ModLoaderType.NeoForge:
                        //Forges = loaders.ToObservableList();
                        break;
                }
            });
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(CurrentModLoader) && CurrentModLoader != null) {
            var loader = CurrentModLoader;

            string loaderInfo = _loaderType switch {
                ModLoaderType.Forge => $"{((ForgeInstallEntry)loader).ForgeVersion}{(string.IsNullOrEmpty(((ForgeInstallEntry)loader).Branch) ? string.Empty : $"-{((ForgeInstallEntry)loader).Branch}")}",
                ModLoaderType.Quilt => ((QuiltInstallEntry)loader).Loader.Version,
                ModLoaderType.Fabric => ((FabricInstallEntry)loader).Loader.Version,
                _ => throw new NotSupportedException()
            };

            CustomGameCoreId = $"{GameCoreId}-{_loaderType}_{loaderInfo}";
        }
    }
}