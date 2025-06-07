using MinecraftLaunch.Base.Models.Authentication;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WonderLab.Classes.Models;

public record SpecificSettingModel {
    [JsonPropertyName("width")] public int Width { get; set; } = 854;
    [JsonPropertyName("height")] public int Height { get; set; } = 480;

    [JsonPropertyName("isFullScreen")] public bool IsFullScreen { get; set; }
    [JsonPropertyName("isEnableIndependency")] public bool IsEnableIndependency { get; set; } = true;
    [JsonPropertyName("isEnableSpecificSetting")] public bool IsEnableSpecificSetting { get; set; }

    [JsonPropertyName("jvmArgument")] public string JvmArgument { get; set; } = string.Empty;
    [JsonPropertyName("serverAddress")] public string ServerAddress { get; set; } = string.Empty;

    [JsonPropertyName("activeAccount")] public Account ActiveAccount { get; set; }
}

[JsonSerializable(typeof(Dictionary<string, SpecificSettingModel>))]
public sealed partial class SpecificSettingModelContext : JsonSerializerContext;