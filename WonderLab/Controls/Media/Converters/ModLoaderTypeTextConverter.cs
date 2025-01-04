using Avalonia.Data.Converters;
using MinecraftLaunch.Classes.Models.Install;
using System;
using System.Globalization;

namespace WonderLab.Controls.Media.Converters;

public sealed class ModLoaderTypeTextConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ForgeInstallEntry forgeInstallEntry) {
            return $"{forgeInstallEntry.ForgeVersion}{(string.IsNullOrEmpty(forgeInstallEntry.Branch) ? string.Empty : $"-{forgeInstallEntry.Branch}")}";
        } else if (value is FabricBuildEntry fabricBuildEntry) {
            return fabricBuildEntry.Loader.Version;
        } else if (value is QuiltBuildEntry quiltBuildEntry) {
            return quiltBuildEntry.Loader.Version;
        }

        return "Not Found";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}