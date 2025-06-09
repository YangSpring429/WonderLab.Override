using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WonderLab.Classes.Nodes;

public partial class OptionsNode : IEnumerable<KeyValuePair<string, object>> {
    private readonly Dictionary<string, object> _data = [];

    public object this[string key] {
        get => _data[key];
        set => _data[key] = value;
    }

    [GeneratedRegex("\"([^\"]*)\"")]
    protected static partial Regex OptionListRegex();

    internal OptionsNode() { }

    public bool TryGetValue(string key, out object value) => _data.TryGetValue(key, out value);

    public static OptionsNode Parse(string text) {
        var lines = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        return Parse(lines);
    }

    public static OptionsNode Parse(IEnumerable<string> lines) {
        var node = new OptionsNode();
        foreach (var rawLine in lines) {
            var line = rawLine.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith('#'))
                continue;

            var idx = line.IndexOf(':');
            if (idx < 0) continue;

            var key = line[..idx].Trim();
            var valueStr = line[(idx + 1)..].Trim();

            var value = ParseValue(valueStr);
            node._data[key] = value;
        }

        return node;

    }

    public T GetValue<T>(string key) {
        if (_data.TryGetValue(key, out var value)) {
            if (value is T t)
                return t;

            throw new InvalidCastException($"Key '{key}' is not of type {typeof(T).Name}.");
        }

        if (!TryAddNode<T>(key, default))
            throw new Exception();

        return default;
    }

    public bool TryAddNode<T>(string key, T value) {
        return _data.TryAdd(key, value);
    }

    public override string ToString() {
        var sb = new StringBuilder();
        foreach (var (k, v) in _data)
            sb.Append(k).Append(':').Append(SerializeValue(v)).AppendLine();

        return sb.ToString();
    }

    private static object ParseValue(string valueStr) {
        if (valueStr.StartsWith('[') && valueStr.EndsWith(']')) {
            var inner = valueStr[1..^1].Trim();
            if (string.IsNullOrEmpty(inner))
                return Enumerable.Empty<string>();

            var items = new List<string>();
            var matches = OptionListRegex().Matches(inner);
            foreach (Match m in matches)
                items.Add(m.Groups[1].Value);

            return items;
        }

        if (valueStr.StartsWith('\"') && valueStr.EndsWith('\"'))
            return valueStr[1..^1];

        if (bool.TryParse(valueStr, out var b))
            return b;

        if (int.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
            return i;

        if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
            return d;

        return valueStr;
    }

    private static string SerializeValue(object value) {
        switch (value) {
            case null: return "";
            case string s: return $"\"{s}\"";
            case bool b: return b.ToString().ToLowerInvariant();
            case IEnumerable arr:
                var items = new List<string>();
                foreach (var item in arr)
                    items.Add($"\"{item}\"");

                return $"[{string.Join(",", items)}]";
            default:
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _data.GetEnumerator();
}