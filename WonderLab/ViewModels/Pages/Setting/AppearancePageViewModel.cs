using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Monet.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WonderLab.Classes.Enums;
using WonderLab.Services;

namespace WonderLab.ViewModels.Pages.Setting;

public sealed partial class AppearancePageViewModel : ObservableObject {
    private readonly ThemeService _themeService;
    private readonly SettingService _settingService;
    private readonly ILogger<AppearancePageViewModel> _logger;

    [ObservableProperty] private string _imagePath;
    [ObservableProperty] private ThemeType _activeTheme;
    [ObservableProperty] private Variant _activeColorVariant;
    [ObservableProperty] private BackgroundType _activeBackground;
    [ObservableProperty] private ReadOnlyObservableCollection<BackgroundType> _backgrounds;

    public AppearancePageViewModel(SettingService settingService, ThemeService themeService, ILogger<AppearancePageViewModel> logger) {
        _logger = logger;
        _themeService = themeService;
        _settingService = settingService;

        Backgrounds = new(_themeService.BackgroundTypes);

        ImagePath = _settingService.Setting.ImagePath;
        ActiveTheme = _settingService.Setting.ActiveTheme;
        ActiveBackground = _settingService.Setting.ActiveBackground;
        ActiveColorVariant = _settingService.Setting.ActiveColorVariant;
    }

    [RelayCommand]
    private void OnLoaded() {
        PropertyChanged += OnPropertyChanged;
    }

    [RelayCommand]
    private Task BrowserImage() => Dispatcher.UIThread.InvokeAsync(async () => {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) {
            var result = await lifetime.MainWindow.StorageProvider.OpenFilePickerAsync(new() {
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.ImageAll]
            });

            if (result is { Count: 0 })
                return;

            ImagePath = result[0].Path.LocalPath;
        }
    });

    private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
        await Dispatcher.UIThread.InvokeAsync(() => {
            switch (e.PropertyName) {
                case nameof(ActiveTheme):
                    _settingService.Setting.ActiveTheme = ActiveTheme;
                    Application.Current.RequestedThemeVariant = ActiveTheme switch {
                        ThemeType.Auto => ThemeVariant.Default,
                        ThemeType.Dark => ThemeVariant.Dark,
                        ThemeType.Light => ThemeVariant.Light,
                        _ => ThemeVariant.Light,
                    };
                    break;
                case nameof(ImagePath):
                    _themeService.UpdateBackgroundType(ActiveBackground, _settingService.Setting.ImagePath = ImagePath);
                    break;
                case nameof(ActiveBackground):
                    _themeService.UpdateBackgroundType(_settingService.Setting.ActiveBackground = ActiveBackground, ImagePath);
                    break;
                case nameof(ActiveColorVariant):
                    _themeService.UpdateColorScheme(_settingService.Setting.ActiveColorVariant = ActiveColorVariant);
                    break;
            }
        }, DispatcherPriority.Background);
    }
}