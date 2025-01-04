using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using MinecraftLaunch.Classes.Models.Install;
using System;

namespace WonderLab.Controls.Media;

[Obsolete]
public sealed class ModLoaderDataTemplateSelector : IDataTemplate {
    public DataTemplate Forge { get; set; }

    public Control Build(object data) {
        if (data is ForgeInstallEntry) {
            return Forge.Build(data);
        }

        return null;
    }

    public bool Match(object data) {
       return data is ForgeInstallEntry || data is FabricBuildEntry;
    }
}