using Avalonia.Media;
using MinecraftLaunch.Base.Models.Authentication;
using MinecraftLaunch.Base.Models.Network;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WonderLab.Services.Download;

public sealed class CacheService {
    public List<VersionManifestEntry> MinecraftList { get; } = [];

    public static Dictionary<MinecraftLaunch.Base.Models.Authentication.Account, IImmutableBrush> SkinAreas { get; }
        = new(new AccountEqualityComparer());

    public void AddSkin(MinecraftLaunch.Base.Models.Authentication.Account account, IImmutableBrush area) {
        if (account is null || SkinAreas.ContainsKey(account)) {
            return;
        }

        SkinAreas.Add(account, area);
    }

    public bool TryGetArea(MinecraftLaunch.Base.Models.Authentication.Account account, out IImmutableBrush area) {
        if (SkinAreas.TryGetValue(account, out var area1)) {
            area = area1;
            return true;
        }

        area = null;
        return false;
    }
}

internal sealed class AccountEqualityComparer : IEqualityComparer<MinecraftLaunch.Base.Models.Authentication.Account> {
    public bool Equals(MinecraftLaunch.Base.Models.Authentication.Account account1, MinecraftLaunch.Base.Models.Authentication.Account account2) {
        return account1.Type == account2.Type
            && account1.Name == account2.Name
            && account1.Uuid == account2.Uuid;
    }

    public int GetHashCode([DisallowNull] MinecraftLaunch.Base.Models.Authentication.Account obj) {
        return obj.GetHashCode();
    }
}