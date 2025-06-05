using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using WonderLab.Services;

namespace WonderLab.ViewModels.Dialogs.Setting;

public sealed partial class ChooseAccountTypeDialogViewModel : DialogViewModelBase {
    private readonly DialogService _dialogService;

    public ChooseAccountTypeDialogViewModel(DialogService dialogService) {
        _dialogService = dialogService;
    }

    [RelayCommand]
    private Task JumpToOfflineAuthDialog() => _dialogService.ShowDialogAsync("OfflineAuth");

    [RelayCommand]
    private Task JumpToYggdrasilAuthDialog() => _dialogService.ShowDialogAsync("YggdrasilAuth");

    [RelayCommand]
    private Task JumpToMicrosoftAuthDialog() => _dialogService.ShowDialogAsync("MicrosoftAuth");
}