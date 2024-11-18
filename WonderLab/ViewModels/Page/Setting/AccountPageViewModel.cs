using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Components.Authenticator;
using System.Collections.Generic;

namespace WonderLab.ViewModels.Page.Setting;

public sealed partial class AccountPageViewModel : ObservableObject {
    public List<Account> Test => [
        new OfflineAuthenticator("Account1").Authenticate(),
        new OfflineAuthenticator("Account2").Authenticate(),
        new OfflineAuthenticator("Account3").Authenticate(),
        new OfflineAuthenticator("Account4").Authenticate(),
        new OfflineAuthenticator("Account5").Authenticate(),
        new OfflineAuthenticator("Account6").Authenticate(),
        new OfflineAuthenticator("Account7").Authenticate(),
    ];
}