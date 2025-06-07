using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Authentication;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Extensions;
using MinecraftLaunch.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Classes.Models;

namespace WonderLab.Classes.Processors;

public sealed class SpecificSettingProcessor : IDataProcessor {
    private string _filePath;
    private Dictionary<string, SpecificSettingModel> _specificSettings;

    public Dictionary<string, object> Datas { get; set; } = [];

    public void Handle(object data) {
        Datas.Clear();

        if (data is IEnumerable<MinecraftEntry> minecrafts && minecrafts.Any()) {
            _filePath = Path.Combine(minecrafts.First()?.MinecraftFolderPath, "wonderlab_profiles.json");
            if (File.Exists(_filePath)) {
                var launcherProfileJson = File.ReadAllText(_filePath, Encoding.UTF8);
                _specificSettings = launcherProfileJson.Deserialize(new SpecificSettingModelContext(
                    JsonSerializerUtil.GetDefaultOptions()).DictionaryStringSpecificSettingModel);
            }

            foreach (var minecraft in minecrafts) {
                _specificSettings ??= [];

                if (!_specificSettings.ContainsKey(minecraft.Id))
                    _specificSettings.Add(minecraft.Id, new());
            }

            Datas = _specificSettings.ToDictionary(x => x.Key, x1 => x1.Value as object);
        }
    }

    public Task SaveAsync(CancellationToken cancellationToken = default) {
        _specificSettings = Datas.ToDictionary(x => x.Key, x1 => x1.Value as SpecificSettingModel);
        var json = _specificSettings?.Serialize(new SpecificSettingModelContext(
            JsonSerializerUtil.GetDefaultOptions()).DictionaryStringSpecificSettingModel);

        return File.WriteAllTextAsync(_filePath, json, cancellationToken);
    }
}