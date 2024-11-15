using MinecraftLaunch.Classes.Interfaces;

namespace WonderLab.Infrastructure.Models.Launch;

public sealed class GameProcess {
    public GameModel Game { get; set; }
    public IGameProcessWatcher ProcessWatcher { get; set; }
}