using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using MinecraftLaunch.Base.Models.Authentication;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using WonderLab.Classes.Models.Messaging;

namespace WonderLab.Services.Authentication;

public sealed class AccountService {
    private readonly SettingService _settingService;
    private readonly ILogger<AccountService> _logger;

    public Account ActiveAccount { get; private set; }
    public ObservableCollection<Account> Accounts { get; private set; }

    public AccountService(SettingService settingService, ILogger<AccountService> logger) {
        _logger = logger;
        _settingService = settingService;

        Accounts = [.. _settingService.Setting.Accounts];
        Accounts.CollectionChanged += OnCollectionChanged;

        ActivateAccount(_settingService.Setting.ActiveAccount);
        _logger.LogInformation("初始化 {name}", nameof(AccountService));
    }

    public bool RemoveAccount(Account account) => Accounts.Remove(account);

    public void AddAccount(Account account) {
        if (Accounts.Any(x => x.Uuid == account.Uuid))
            return;

        Accounts.Add(account);
    }

    public void ActivateAccount(Account account) {
        _logger.LogInformation("选择账户：{account} - {type}", account?.Name, account?.Type);

        if (account != null && !Accounts.Contains(account))
            return;

        _settingService.Setting.ActiveAccount = ActiveAccount = account;
        WeakReferenceMessenger.Default.Send(new ActiveAccountChangedMessage());
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        switch (e.Action) {
            case NotifyCollectionChangedAction.Add:
                _settingService.Setting.Accounts.Add(Accounts.Last());
                break;
            case NotifyCollectionChangedAction.Remove:
                _settingService.Setting.Accounts.Remove(e.OldItems[0] as Account);
                break;
        }
    }
}