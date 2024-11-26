using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Components.Authenticator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WonderLab.Services.Accounts;

public sealed class AccountService {
    private const string CLIENT_ID = "9fd44410-8ed7-4eb3-a160-9f1cc62c824c";

    private readonly ConfigService _configService;
    private readonly MicrosoftAuthenticator _microsoftAuthenticator;

    private ObservableCollection<Account> _accounts;

    public event EventHandler CollectionChanged;

    public ReadOnlyObservableCollection<Account> Accounts { get; set; }

    public AccountService(ConfigService configService) {
        _configService = configService;
        _microsoftAuthenticator = new(CLIENT_ID);
    }

    public void Initialize() {
        _accounts = new(_configService?.Entries?.Accounts ?? []);
        Accounts = new(_accounts);
    }

    public OfflineAccount CreateOfflineAccount(string name) {
        var account = new OfflineAuthenticator(name)
            .Authenticate();

        _accounts.Add(account);
        _configService.Entries.Accounts.Add(account);

        CollectionChanged?.Invoke(this, EventArgs.Empty);
        return account;
    }

    public async ValueTask<IEnumerable<YggdrasilAccount>> CreateYggdrasilAccounts(string email, string password, string url = "https://littleskin.cn/api/yggdrasil") {
        var accounts = (await new YggdrasilAuthenticator(url, email, password)
            .AuthenticateAsync()).ToList();

        accounts.ForEach(_accounts.Add);
        _configService.Entries.Accounts.AddRange(accounts);

        CollectionChanged?.Invoke(this, EventArgs.Empty);
        return accounts;
    }

    public async Task<MicrosoftAccount> CreateMicrosoftAccount(Action<DeviceCodeResponse> action, CancellationTokenSource cancellationTokenSource) {
        var resultOAuth2 = await _microsoftAuthenticator
            .DeviceFlowAuthAsync(action, cancellationTokenSource);

        var account = await _microsoftAuthenticator.AuthenticateAsync();

        _accounts.Add(account);
        _configService.Entries.Accounts.Add(account);

        CollectionChanged?.Invoke(this, EventArgs.Empty);
        return account;
    }
}