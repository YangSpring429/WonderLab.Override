using MinecraftLaunch.Base.Models.Game;

namespace WonderLab.Infrastructure.Models.Launch;

public record GameModel {
    public MinecraftEntry Entry { get; }
    public GameJsonModel Model { set; get; }

    public GameModel(MinecraftEntry entry, GameJsonModel model) {
        Entry = entry;
        Model = model;
    }
}

public record GameJsonModel {
    public bool IsCollection { set; get; }
    public string Id { set; get; }
    public string MinecraftFolder { set; get; }
}