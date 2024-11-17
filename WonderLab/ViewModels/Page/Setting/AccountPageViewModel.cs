using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace WonderLab.ViewModels.Page.Setting;

public sealed partial class AccountPageViewModel : ObservableObject {
    public List<string> Test => [
        "Account1",
        "Account2",
        "Account3",
        "Account4",
        "Account5",
        "Account6",
        "Account7",
    ];
}