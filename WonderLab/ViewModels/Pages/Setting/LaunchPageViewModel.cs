using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WonderLab.Services;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Pages.Setting;

public sealed partial class LaunchPageViewModel : ObservableObject {
    private readonly GameService _gameService;
    private readonly SettingService _settingService;
    private readonly ObservableCollection<string> _mcFolders;

    [ObservableProperty] private bool _isFullScreen;
    [ObservableProperty] private bool _isEnableIndependency;

    [ObservableProperty] private int _width;
    [ObservableProperty] private int _height;
    [ObservableProperty] private string _serverAddress;
    [ObservableProperty] private string _activeMinecraftFolder;

    public ReadOnlyObservableCollection<string> MinecraftFolders { get; }

    public LaunchPageViewModel(SettingService settingService, GameService gameService) {
        _gameService = gameService;
        _settingService = settingService;
        MinecraftFolders = new(_mcFolders = [.. _settingService.Setting.MinecraftFolders]);

        Width = _settingService.Setting.Width;
        Height = _settingService.Setting.Height;

        ServerAddress = _settingService.Setting.ServerAddress;
        ActiveMinecraftFolder = _settingService.Setting.ActiveMinecraftFolder;

        IsFullScreen = _settingService.Setting.IsFullScreen;
        IsEnableIndependency = _settingService.Setting.IsEnableIndependency;

        _mcFolders.CollectionChanged += OnCollectionChanged;
    }

    [RelayCommand]
    private Task BrowserFolder() => Dispatcher.UIThread.InvokeAsync(async () => {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) {
            var result = await lifetime.MainWindow.StorageProvider.OpenFolderPickerAsync(new() {
                AllowMultiple = false,
            });

            if (result is { Count: 0 })
                return;

            var path = result[0].Path.LocalPath;
            _mcFolders.Add(path);
            ActiveMinecraftFolder = _mcFolders.Last();
        }
    }, DispatcherPriority.Background);

    [RelayCommand]
    private void RemoveFolder() {
        if (_mcFolders.Remove(ActiveMinecraftFolder))
            ActiveMinecraftFolder = _mcFolders.FirstOrDefault();
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        switch (e.Action) {
            case NotifyCollectionChangedAction.Add:
                _settingService.Setting.MinecraftFolders.Add(_mcFolders.Last());
                break;
            case NotifyCollectionChangedAction.Remove:
                _settingService.Setting.MinecraftFolders.Remove(e.OldItems[0].ToString());
                break;
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        switch (e.PropertyName) {
            case nameof(ActiveMinecraftFolder):
                _gameService.ActivateMinecraftFolder(ActiveMinecraftFolder);
                break;
            case nameof(Width):
                _settingService.Setting.Width = Width;
                break;
            case nameof(Height):
                _settingService.Setting.Height = Height;
                break;
            case nameof(ServerAddress):
                _settingService.Setting.ServerAddress = ServerAddress;
                break;
            case nameof(IsFullScreen):
                _settingService.Setting.IsFullScreen = IsFullScreen;
                break;
            case nameof(IsEnableIndependency):
                _settingService.Setting.IsEnableIndependency = IsEnableIndependency;
                break;
        }
    }
}