using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using System.Threading.Tasks;
using WonderLab.ViewModels.Dialog.Auth;
using WonderLab.Views.Dialog.Auth;

namespace WonderLab.ViewModels.Dialog;

public sealed partial class ChooseAccountTypeDialogViewModel : ObservableObject {
    [RelayCommand]
    private void Close() {
        DialogHost.Close("PART_DialogHost");
    }

    [RelayCommand]
    private Task GoToOfflineAuth() => Dispatcher.UIThread.InvokeAsync(async () => {
        Close();

        OfflineAuthDialog dialog = new() {
            DataContext = App.Get<OfflineAuthDialogViewModel>()
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });

    [RelayCommand]
    private Task GoToYggdrasilAuth() => Dispatcher.UIThread.InvokeAsync(async () => {
        Close();

        YggdrasilAuthDialog dialog = new() {
            DataContext = App.Get<YggdrasilAuthDialogViewModel>()
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });

    [RelayCommand]
    private Task GoToMicrosoftAuth() => Dispatcher.UIThread.InvokeAsync(async () => {
        Close();

        MicrosoftAuthDialog dialog = new() {
            DataContext = App.Get<MicrosoftAuthDialogViewModel>()
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });
}