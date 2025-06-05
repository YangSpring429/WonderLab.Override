using MinecraftLaunch.Base.Models.Game;
using System;
using System.Text.Json.Serialization;

namespace WonderLab.Classes.Models;

[JsonDerivedType(typeof(MultiPlayerSaveModel), typeDiscriminator: "multiPlayer")]
[JsonDerivedType(typeof(SinglePlayerSaveModel), typeDiscriminator: "singlePlayer")]
public abstract record SaveModel {
    [JsonPropertyName("name")] public virtual string Name { get; set; }
    [JsonPropertyName("minecraftId")] public string MinecraftId { get; set; }
    [JsonPropertyName("lastPlayedTime")] public abstract DateTime? LastPlayedTime { get; set; }
}

public record SinglePlayerSaveModel : SaveModel {
    [JsonPropertyName("iconPath")] public string IconPath { get; set; }
    [JsonPropertyName("metaData")] public SaveEntry MetaData { get; set; }

    public override string Name {
        get => MetaData?.LevelName;
        set => MetaData.LevelName = value;
    }

    public override DateTime? LastPlayedTime {
        get => MetaData?.LastPlayed;
        set => MetaData.LastPlayed = value!.Value;
    }
}

public record MultiPlayerSaveModel : SaveModel {
    [JsonPropertyName("icon")] public string Icon { get; set; }
    [JsonPropertyName("ipAddress")] public string IPAddress { get; set; }
    [JsonPropertyName("minecraftFolder")] public string MinecraftFolder { get; init; }

    public override DateTime? LastPlayedTime { get; set; }
}