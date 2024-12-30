using MinecraftLaunch.Classes.Enums;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch.Components.Installer;
using System.Threading.Tasks;
using WonderLab.Services.Launch;

namespace WonderLab.Services.Download;

public sealed class DownloadService {
    private readonly GameService _gameService;
    private readonly ConfigService _configService;

    public DownloadService(ConfigService configService, GameService gameService) {
        _gameService = gameService;
        _configService = configService;
    }

    public async Task InstallMinecraftAsync(LoaderType type, string gameId, string customGameId = default) {
        var dc = new DownloaderConfiguration {
            IsEnableFragmentedDownload = true,
            MaxThread = _configService.Entries.ThreadCount
        };
        //CompositionInstaller
        //var installer = type switch {
        //    LoaderType.Any => new VanlliaInstaller(_gameService.GameResolver, gameId, dc),
        //    LoaderType.Forge => new ForgeInstaller(_gameService.GameResolver.GetGameEntity())
        //};
    }
}