using Avalonia.Data.Converters;
using System;
using System.Globalization;
using MinecraftLaunch.Base.Models.Network;

namespace WonderLab.Controls.Media.Converters;

public sealed class ModLoaderTypeTextConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is ForgeInstallEntry forgeInstallEntry) {
            return $"{forgeInstallEntry.ForgeVersion}{(string.IsNullOrEmpty(forgeInstallEntry.Branch) ? string.Empty : $"-{forgeInstallEntry.Branch}")}";
        } else if (value is FabricInstallEntry fabricBuildEntry) {
            return fabricBuildEntry.Loader.Version;
        } else if (value is QuiltInstallEntry quiltBuildEntry) {
            return quiltBuildEntry.Loader.Version;
        }

        return "Not Found";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}