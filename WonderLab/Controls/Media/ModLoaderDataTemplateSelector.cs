using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using System;
using MinecraftLaunch.Base.Models.Network;

namespace WonderLab.Controls.Media;

[Obsolete]
public sealed class ModLoaderDataTemplateSelector : IDataTemplate {
    public DataTemplate Forge { get; set; }

    public Control Build(object data) {
        return data is ForgeInstallEntry ? Forge.Build(data) : null;
    }

    public bool Match(object data) {
       return data is ForgeInstallEntry or FabricInstallEntry;
    }
}