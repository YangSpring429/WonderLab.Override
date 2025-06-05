using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using MinecraftLaunch.Base.Utilities;
using Monet.Shared.Enums;
using System;
using System.Collections.ObjectModel;
using WonderLab.Classes.Enums;
using WonderLab.Controls;
using WonderLab.Extensions;

namespace WonderLab.Services;

public sealed class ThemeService {
    private readonly SettingService _settingService;
    private readonly ILogger<ThemeService> _logger;

    private WonderWindow _hostWindow;

    public static readonly Lazy<Bitmap> ReleaseMinecraftIcon = new("resm:WonderLab.Assets.Images.Icons.release_minecraft.png".ToBitmap());

    public ObservableCollection<BackgroundType> BackgroundTypes { get; private set; }

    public event EventHandler BackgroundTypeChanged;

    public ThemeService(SettingService settingService, ILogger<ThemeService> logger) {
        _logger = logger;
        _settingService = settingService;
    }

    public void Initialize(WonderWindow window) {
        _hostWindow = window;

        BackgroundTypes = [
            BackgroundType.SolidColor,
            BackgroundType.Bitmap,
            BackgroundType.Voronoi,
            BackgroundType.Bubble
        ];

        if (EnvironmentUtil.IsMac) {
            BackgroundTypes.Add(BackgroundType.Acrylic);
        } else if (EnvironmentUtil.IsWindow) {
            BackgroundTypes.Add(BackgroundType.Mica);
            BackgroundTypes.Add(BackgroundType.Acrylic);
        }

        Dispatcher.UIThread.Post(() => {
            UpdateThemeVariant(_settingService.Setting.ActiveTheme);
            UpdateColorScheme(_settingService.Setting.ActiveColorVariant);
            UpdateBackgroundType(_settingService.Setting.ActiveBackground, _settingService.Setting.ImagePath);
        });
    }

    public void UpdateColorScheme(Variant variant) {
        _logger.LogInformation("切换动态颜色变种，类别：{variant}", variant.ToString());
        App.Monet.BuildScheme(variant, Color.Parse("#579CC5"), 0);
    }

    public void UpdateThemeVariant(ThemeType type) {
        var variant = type switch {
            ThemeType.Dark => ThemeVariant.Dark,
            ThemeType.Light => ThemeVariant.Light,
            ThemeType.Auto => ThemeVariant.Default,
            _ => ThemeVariant.Light,
        };

        Application.Current.RequestedThemeVariant = variant;
    }

    public void UpdateThemeVariant(ThemeVariant variant, Variant colorVariant = Variant.Tonal_Spot) {
        _logger.LogInformation("切换程序主题，类别：{variant}", variant.ToString());

        Application.Current.RequestedThemeVariant = variant;

        App.Monet.IsDarkMode = variant == ThemeVariant.Dark;
        App.Monet.BuildScheme(colorVariant, Colors.Red, 0);
    }

    public void UpdateBackgroundType(BackgroundType type, string imagePath = default) {
        _logger.LogInformation("切换程序背景主题，类别：{type}", type);

        if (!string.IsNullOrEmpty(imagePath)) {
            _hostWindow.ImagePath = imagePath;
        }

        _hostWindow.BackgroundType = type;
        BackgroundTypeChanged?.Invoke(this, EventArgs.Empty);
    }
}