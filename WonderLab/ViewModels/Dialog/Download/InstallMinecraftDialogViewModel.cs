using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using MinecraftLaunch.Classes.Enums;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services.Download;

namespace WonderLab.ViewModels.Dialog.Download;

public sealed partial class InstallMinecraftDialogViewModel : ObservableObject {
    private readonly DownloadService _downloadService;

    private LoaderType _loaderType;

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
            "Not" => LoaderType.Any,
            "Forge" => LoaderType.Forge,
            "Quilt" => LoaderType.Quilt,
            "Fabric" => LoaderType.Fabric,
            "Neoforge" => LoaderType.Neoforge,
            _ => LoaderType.Any,
        };
    }

    private Task LoadModLoaders() {
        var tasks = new List<Task>() {
            Task.Run(() => LoadModLoader(LoaderType.Quilt)),
            Task.Run(() => LoadModLoader(LoaderType.Forge)),
            Task.Run(() => LoadModLoader(LoaderType.Fabric)),
            Task.Run(() => LoadModLoader(LoaderType.Neoforge)),
            Task.Run(() => LoadModLoader(LoaderType.OptiFine)),
        };

        return Task.WhenAll(tasks);

        async void LoadModLoader(LoaderType type) {
            IEnumerable<object> loaders = type switch {
                LoaderType.Forge => await ForgeInstaller.EnumerableFromVersionAsync(GameCoreId),
                LoaderType.Quilt => await QuiltInstaller.EnumerableFromVersionAsync(GameCoreId),
                LoaderType.Fabric => await FabricInstaller.EnumerableFromVersionAsync(GameCoreId),
                LoaderType.OptiFine => await OptifineInstaller.EnumerableFromVersionAsync(GameCoreId),
                _ => null
            };

            Dispatcher.UIThread.Post(() => {
                switch (type) {
                    case LoaderType.Forge:
                        Forges = loaders.ToObservableList();
                        break;
                    case LoaderType.Quilt:
                        Quilts = loaders.ToObservableList();
                        break;
                    case LoaderType.Fabric:
                        Fabrics = loaders.ToObservableList();
                        break;
                    case LoaderType.OptiFine:
                        Optifines = loaders.ToObservableList();
                        break;
                    case LoaderType.Neoforge:
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
                LoaderType.Forge => $"{((ForgeInstallEntry)loader).ForgeVersion}{(string.IsNullOrEmpty(((ForgeInstallEntry)loader).Branch) ? string.Empty : $"-{((ForgeInstallEntry)loader).Branch}")}",
                LoaderType.Quilt => ((QuiltBuildEntry)loader).Loader.Version,
                LoaderType.Fabric => ((FabricBuildEntry)loader).Loader.Version,
                _ => throw new NotSupportedException()
            };

            CustomGameCoreId = $"{GameCoreId}-{_loaderType}_{loaderInfo}";
        }
    }
}