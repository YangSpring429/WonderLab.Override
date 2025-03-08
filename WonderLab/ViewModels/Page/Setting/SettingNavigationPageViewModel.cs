using Avalonia.Markup.Xaml.MarkupExtensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WonderLab.Controls.Experimental.BreadcrumbBar;
using WonderLab.Extensions.Hosting.UI;

namespace WonderLab.ViewModels.Page.Setting;
public sealed partial class SettingNavigationPageViewModel : ObservableObject {
    public AvaloniaPageProvider PageProvider { get; }

    [ObservableProperty] private string _activePageKey;
    [ObservableProperty] private int _activePageIndex = -1;
    [ObservableProperty] private bool _isHide = true;

    [ObservableProperty] private ObservableCollection<string> _testList = [I18NExtension.Translate(LanguageKeys.Main_Settings, "Settings")];

    public SettingNavigationPageViewModel(AvaloniaPageProvider avaloniaPageProvider) {
        PageProvider = avaloniaPageProvider;
    }

    [RelayCommand]
    private void OnLoaded() {
        ActivePageIndex = 0;
    }

    [RelayCommand]
    private void ChangeActivePage(object index) {
        if (TestList.Count == 2)
            return;

        var intIndex = Convert.ToInt32(index);
        ActivePageKey = intIndex switch {
            0 => "Setting/Launch",
            1 => "Setting/Account",
            2 => "Setting/Network",
            3 => "Setting/Appearance",
            4 => "Setting/About",
            _ => "Setting/Launch"
        };

        IsHide = false;
        TestList.Add(I18NExtension.Translate(Convert.ToInt32(index) switch {
            0 => LanguageKeys.Nav_Settings_Launch,
            1 => LanguageKeys.Nav_Settings_Account,
            2 => LanguageKeys.Nav_Settings_Network,
            3 => LanguageKeys.Nav_Settings_Appearance,
            4 => LanguageKeys.Nav_Settings_About,
            _ => "Unknown"
        }, "Launch"));
    }

    [RelayCommand]
    private void OnItemClicked(BreadcrumbBarItemClickedEventArgs arg) {
        if (TestList.Count != 2 || arg.Index != 0)
            return;
        
        IsHide = true;
        TestList.Remove(TestList.Last());
    } 
}