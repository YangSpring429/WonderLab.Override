using Microsoft.Extensions.Logging;
using MinecraftLaunch.Extensions;
using MinecraftLaunch.Utilities;
using System;
using System.IO;
using System.Text.Json;
using WonderLab.Infrastructure.Models;

namespace WonderLab.Services;

public sealed class ConfigService {
    private const string CONFIG_FILENAME = "wl_config.json";
    private readonly ILogger<ConfigService> _logger;

    public Config Entries { get; private set; }

    public ConfigService(ILogger<ConfigService> logger) {
        _logger = logger;
    }

    public void Load() {
        _logger.LogInformation("Loading config json");

        try {
            var json = File.ReadAllText(CONFIG_FILENAME);
            Entries = json.Deserialize(ConfigContext.Default.Config);
        } catch (Exception ex) {
            _logger.LogError("Failed to load config json.\n{Trace}", ex.ToString());

            Save();
            Entries = new Config();
        }

        _logger.LogInformation("Check if the data is available");
        if (Entries.MaxMemory == 0) {
            Entries.MaxMemory = 1024;
        }

        if (string.IsNullOrWhiteSpace(Entries.ActiveLanguage)) {
            Entries.ActiveLanguage = "zh-Hans";
        }

        Entries.Javas ??= [];
        Entries.Accounts ??= [];
        Entries.MinecraftFolders ??= [];
    }

    public void Save() {
        _logger.LogInformation("Saving config json");

        try {
            var json = Entries.Serialize(ConfigContext.Default.Config);
            File.WriteAllText(CONFIG_FILENAME, json);
        } catch (Exception ex) {
            _logger.LogError("Failed to save config json.\n{Trace}", ex.ToString());
        }
    }
}