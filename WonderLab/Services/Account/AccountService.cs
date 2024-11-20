using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Components.Authenticator;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WonderLab.Services.Accounts;

public sealed class AccountService {
    private readonly ConfigService _configService;

    public ObservableCollection<Account> Accounts { get; set; }

    public AccountService(ConfigService configService) {
        _configService = configService;
    }

    public void Initialize() {
        Accounts = new(_configService?.Entries?.Accounts ?? []);
    }

    public OfflineAccount CreateOfflineAccount(string name) {
        var account = new OfflineAuthenticator(name)
            .Authenticate();

        _configService.Entries.Accounts.Add(account);
        return account;
    }

    public IEnumerable<YggdrasilAccount> CreateYggdrasilAccounts(string email, string password, string url = "https://littleskin.cn/api/yggdrasil") {
        var accounts = new YggdrasilAuthenticator(url, email, password)
            .Authenticate();

        _configService.Entries.Accounts.AddRange(accounts);
        return accounts;
    }
}