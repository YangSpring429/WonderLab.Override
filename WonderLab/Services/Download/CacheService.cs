using Avalonia.Media;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Classes.Models.Install;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WonderLab.Services.Download;

public sealed class CacheService {
    public List<VersionManifestEntry> MinecraftList { get; } = [];

    public static Dictionary<Account, IImmutableBrush> SkinAreas { get; }
        = new(new AccountEqualityComparer());

    public void AddSkin(Account account, IImmutableBrush area) {
        if (account is null || SkinAreas.ContainsKey(account)) {
            return;
        }

        SkinAreas.Add(account, area);
    }

    public bool TryGetArea(Account account, out IImmutableBrush area) {
        if (SkinAreas.TryGetValue(account, out var area1)) {
            area = area1;
            return true;
        }

        area = null;
        return false;
    }
}

internal sealed class AccountEqualityComparer : IEqualityComparer<Account> {
    public bool Equals(Account account1, Account account2) {
        return account1.Type == account2.Type
            && account1.Name == account2.Name
            && account1.Uuid == account2.Uuid;
    }

    public int GetHashCode([DisallowNull] Account obj) {
        return obj.GetHashCode();
    }
}