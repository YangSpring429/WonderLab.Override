using Avalonia.Data.Converters;
using MinecraftLaunch.Classes.Models.Install;
using System;
using System.Globalization;
using WonderLab.Services.UI;

namespace WonderLab.Controls.Media.Converters;


public sealed class ModLoaderTypeIconConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ForgeInstallEntry) {
            return ThemeService.LoaderForgeIcon.Value;
        } else if (value is FabricBuildEntry) {
            return ThemeService.LoaderFabricIcon.Value;
        } else if (value is QuiltBuildEntry) {
            return ThemeService.LoaderQuiltIcon.Value;
        }

        return ThemeService.ReleaseMinecraftIcon.Value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}