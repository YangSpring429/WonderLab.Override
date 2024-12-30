using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using MinecraftLaunch.Classes.Enums;
using System.Threading.Tasks;

namespace WonderLab.ViewModels.Dialog.Download;

public sealed partial class InstallMinecraftDialogViewModel : ObservableObject {
    private LoaderType _loaderType;

    [ObservableProperty] private bool _isInstallOptifine;
    [ObservableProperty] private string _customGameCoreId;

    public string GameCoreId { get; set; }

    [RelayCommand]
    private void Close() => DialogHost.Close("PART_DialogHost");

    [RelayCommand]
    private Task Install() => Task.Run(() => {
        
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
}