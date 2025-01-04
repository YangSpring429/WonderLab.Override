using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftLaunch.Classes.Enums;
using System.Threading.Tasks;

namespace WonderLab.ViewModels.Dialog.Download;

internal sealed partial class ChooseModLoaderViewModel : ObservableObject {
    private readonly string _minecraftId;

    [ObservableProperty] private LoaderType _loaderType;

    public ChooseModLoaderViewModel(string minecraftId, LoaderType loaderType) {
        _minecraftId = minecraftId;
        LoaderType = loaderType;
    }

    private async ValueTask LoadModLoader() {
    }
}