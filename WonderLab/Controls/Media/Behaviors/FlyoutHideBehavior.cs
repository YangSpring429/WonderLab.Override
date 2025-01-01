using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactions.Custom;
using System.Reactive;
using System.Reactive.Disposables;

namespace WonderLab.Controls.Media.Behaviors;

/// <summary>
/// 在按钮失焦时自动隐藏 Flyout
/// </summary>
public sealed class FlyoutHideBehavior : DisposingBehavior<Button> {
    protected override void OnAttached(CompositeDisposable disposables) {
        if (AssociatedObject is not null) {
            disposables.Add(AssociatedObject.GetObservable(InputElement.IsFocusedProperty)
                .Subscribe(new AnonymousObserver<bool>(
                    focused => {
                        if (!focused) {
                            AssociatedObject.Flyout?.Hide();
                        }
                    })));
        }
    }
}