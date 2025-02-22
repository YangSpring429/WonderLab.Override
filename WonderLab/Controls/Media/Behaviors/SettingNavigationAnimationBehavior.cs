using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Rendering.Composition;
using Avalonia.Xaml.Interactivity;
using System;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class SettingNavigationAnimationBehavior : Behavior<Visual> {
    public static readonly StyledProperty<bool> IsHideProperty =
        AvaloniaProperty.Register<SettingNavigationAnimationBehavior, bool>(nameof(IsHide));

    public static readonly StyledProperty<double> MaxWidthProperty =
        AvaloniaProperty.Register<SettingNavigationAnimationBehavior, double>(nameof(MaxWidth));

    public bool IsHide {
        get => GetValue(IsHideProperty);
        set => SetValue(IsHideProperty, value);
    }

    public double MaxWidth {
        get => GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    private void RunAnimation() {
        var compositionVisual = ElementComposition.GetElementVisual(AssociatedObject);
        if(compositionVisual is null)
            return;

        var compositor = compositionVisual?.Compositor;
        var opacityAni = compositor?.CreateScalarKeyFrameAnimation();

        opacityAni!.Duration = TimeSpan.FromSeconds(0.45);
        opacityAni.InsertKeyFrame(0f, IsHide ? 1 : 0, new ExponentialEaseOut());
        opacityAni.InsertKeyFrame(1f, IsHide ? 0 : 1, new ExponentialEaseOut());

        compositionVisual!.StartAnimation("Opacity", opacityAni);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
        base.OnPropertyChanged(change);

        if (change.Property == IsHideProperty)
            RunAnimation();
    }
}