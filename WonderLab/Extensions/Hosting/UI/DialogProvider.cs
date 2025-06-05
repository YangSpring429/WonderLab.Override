using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace WonderLab.Extensions.Hosting.UI;

public abstract class DialogProvider<TContent> {
    protected readonly IServiceProvider _dialogProvider;
    protected readonly IReadOnlyDictionary<string, DialogDescriptor> _registeredDialogs;

    public IReadOnlyDictionary<string, DialogDescriptor> RegisteredDialogs => _registeredDialogs;

    public DialogProvider(IReadOnlyDictionary<string, DialogDescriptor> registeredDialogs, IServiceProvider dialogProvider) {
        _registeredDialogs = registeredDialogs;
        _dialogProvider = dialogProvider;
    }

    public object GetDialog(string key) {
        var dialogType = _registeredDialogs[key].DialogType;
        var vmType = _registeredDialogs[key].ViewModelType;

        if (vmType is null) {
            return _dialogProvider.GetRequiredService(dialogType);
        } else {
            var dialog = (TContent)_dialogProvider.GetRequiredService(dialogType);
            var vm = _dialogProvider.GetRequiredService(vmType);
            ConfigureViewModel(dialog, vm);
            return dialog;
        }
    }

    public object GetViewModel(string key) {
        var vmType = RegisteredDialogs[key].ViewModelType;

        if (vmType is null) {
            return null;
        }

        return _dialogProvider.GetRequiredService(vmType);
    }

    protected abstract void ConfigureViewModel(TContent dialog, object viewModel);
}

public sealed class AvaloniaDialogProvider : DialogProvider<UserControl> {
    public AvaloniaDialogProvider(
        IReadOnlyDictionary<string, DialogDescriptor> registeredDialogs,
        IServiceProvider serviceProvider)
        : base(registeredDialogs, serviceProvider) {

    }

    protected override void ConfigureViewModel(UserControl dialog, object viewModel) =>
        dialog.DataContext = viewModel;
}