using MinecraftLaunch.Classes.Models.Auth;
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
}