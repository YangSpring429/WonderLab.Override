using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MinecraftLaunch.Base.Models.Game;
using System.Threading.Tasks;
using WonderLab.Extensions.Hosting.UI;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Pages.GameSetting;

public sealed partial class GameSettingNavigationPageViewModel : DynamicPageViewModelBase {
    private readonly GameService _gameService;
    private readonly MinecraftEntry _minecraftEntry;

    [ObservableProperty] private string _pageKey;

    public string MinecraftId => _minecraftEntry.Id;
    public AvaloniaPageProvider PageProvider { get; }

    public GameSettingNavigationPageViewModel(GameService gameService, AvaloniaPageProvider avaloniaPageProvider) {
        _gameService = gameService;
        _minecraftEntry = _gameService.ActiveGameCache;

        PageProvider = avaloniaPageProvider;
    }

    [RelayCommand]
    private async Task OnLoaded() {
        await Task.Delay(160);
        PageKey = "GameSetting/Setting";
    }
}