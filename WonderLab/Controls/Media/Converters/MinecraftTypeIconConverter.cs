using Avalonia.Data.Converters;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Base.Models.Network;
using System;
using System.Globalization;
using WonderLab.Services.UI;

namespace WonderLab.Controls.Media.Converters;

public sealed class MinecraftTypeIconConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is MinecraftEntry minecraft) {
            return minecraft.IsVanilla ? minecraft.Version.Type switch {
                MinecraftVersionType.OldBeta => ThemeService.OldMinecraftIcon.Value,
                MinecraftVersionType.OldAlpha => ThemeService.OldMinecraftIcon.Value,
                MinecraftVersionType.Release => ThemeService.ReleaseMinecraftIcon.Value,
                MinecraftVersionType.Snapshot => ThemeService.SnapshotMinecraftIcon.Value,
                _ => null
            } : ThemeService.LoaderMinecraftIcon.Value;
        }

        if (value is VersionManifestEntry) {
            var model = value as VersionManifestEntry;
            return model.Type switch {
                "old_beta" => ThemeService.OldMinecraftIcon.Value,
                "old_alpha" => ThemeService.OldMinecraftIcon.Value,
                "release" => ThemeService.ReleaseMinecraftIcon.Value,
                "snapshot" => ThemeService.SnapshotMinecraftIcon.Value,
                _ => null
            };
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}