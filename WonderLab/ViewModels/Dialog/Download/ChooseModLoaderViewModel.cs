using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using MinecraftLaunch.Base.Enums;

namespace WonderLab.ViewModels.Dialog.Download;

internal sealed partial class ChooseModLoaderViewModel : ObservableObject {
    private readonly string _minecraftId;

    [ObservableProperty] private ModLoaderType _loaderType;

    public ChooseModLoaderViewModel(string minecraftId, ModLoaderType loaderType) {
        _minecraftId = minecraftId;
        LoaderType = loaderType;
    }

    private async ValueTask LoadModLoader() {
    }
}