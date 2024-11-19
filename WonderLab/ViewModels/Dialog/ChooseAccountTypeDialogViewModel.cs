using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using System.Threading.Tasks;

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
            DataContext = App.Get<OfflineAuthDialogViewMdoel>()
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });
}