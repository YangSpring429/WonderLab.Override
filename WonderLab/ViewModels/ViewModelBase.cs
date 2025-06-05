using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DialogHostAvalonia;
using WonderLab.Classes.Models.Messaging;
using WonderLab.Controls;

namespace WonderLab.ViewModels;

public partial class DialogViewModelBase : ObservableObject {
    [RelayCommand]
    public virtual void Close() => DialogHost.Close("Host");
}

public partial class DynamicPageViewModelBase : ObservableObject {
    [RelayCommand]
    public virtual void Close() =>
        WeakReferenceMessenger.Default.Send(new DynamicPageCloseNotificationMessage());
}