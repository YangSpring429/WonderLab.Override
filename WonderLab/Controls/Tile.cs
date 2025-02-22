using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using Avalonia.Styling;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WonderLab.Controls;

public sealed class Tile : ListBoxItem {
    private int _index;
    private int _totalCount;
    private TimeSpan _delay;
    private IChildIndexProvider _childIndexProvider;

    public static readonly StyledProperty<bool> IsAnimationTurnProperty =
        AvaloniaProperty.Register<Tile, bool>(nameof(IsAnimationTurn), false);

    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<Tile, ICommand>(nameof(Command));

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<Tile, object>(nameof(CommandParameter));

    public bool IsAnimationTurn {
        get => GetValue(IsAnimationTurnProperty);
        set => SetValue(IsAnimationTurnProperty, value);
    }

    public ICommand Command {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static void RunParentPanelAnimation(Visual visual, bool isAniTurn = false) {
        var composition = ElementComposition.GetElementVisual(visual);
        var compositor = composition?.Compositor;

        var offsetAni = compositor?.CreateVector3KeyFrameAnimation();
        offsetAni!.Duration = TimeSpan.FromSeconds(0.7);

        if (isAniTurn) {
            offsetAni.InsertKeyFrame(0f, new(0, 100, 0), new ExponentialEaseOut());
            offsetAni.InsertKeyFrame(1f, Vector3.Zero, new ExponentialEaseOut());
        } else {
            offsetAni.InsertKeyFrame(0f, new(100, 0, (float)composition.Offset.Z), new ExponentialEaseOut());
            offsetAni.InsertKeyFrame(1f, Vector3.Zero, new ExponentialEaseOut());
        }

        composition!.StartAnimation("Offset", offsetAni);
    }

    private void RunAnimation() {
        var compositionVisual = ElementComposition.GetElementVisual(this);
        if (compositionVisual is null)
            return;
        
        var compositor = compositionVisual.Compositor;

        var scaleAni = compositor!.CreateVector3KeyFrameAnimation();
        scaleAni.Target = "Scale";
        scaleAni.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
        scaleAni!.Duration = TimeSpan.FromSeconds(0.60);
        scaleAni.DelayTime = _delay;

        scaleAni.InsertKeyFrame(0f, GetScaleOffset(_index, _totalCount), new ExponentialEaseOut());
        scaleAni.InsertKeyFrame(1f, Vector3.One, new ExponentialEaseOut());

        var opacityAni = compositor?.CreateScalarKeyFrameAnimation();
        opacityAni!.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
        opacityAni!.Target = "Opacity";

        opacityAni?.InsertKeyFrame(0f, 0f, new CubicEaseOut());
        opacityAni?.InsertKeyFrame(1f, 1f, new CubicEaseOut());
        opacityAni!.Duration = TimeSpan.FromMicroseconds(10 * (_delay.TotalMicroseconds is <= 0 ? 200 : _delay.TotalMicroseconds));
        opacityAni.DelayTime = _delay;

        var group = compositor!.CreateAnimationGroup();
        group.Add(scaleAni);
        group.Add(opacityAni);

        var size = compositionVisual!.Size;
        if (IsAnimationTurn)
            compositionVisual!.CenterPoint = new Vector3((float)size.X / 2, (float)size.Y, (float)compositionVisual.CenterPoint.Z);
        else
            compositionVisual!.CenterPoint = new Vector3((float)size.X, (float)size.Y / 2, (float)compositionVisual.CenterPoint.Z);


        compositionVisual?.StartAnimationGroup(group);
    }

    private static Vector3 GetScaleOffset(int index, int total, double minOffset = 0.5) {
        double maxOffset = 0.75;
        double step = (maxOffset - minOffset) / (total - 1);

        return new((float)(maxOffset - index * step));
    }

    protected override async void OnLoaded(RoutedEventArgs e) {
        base.OnLoaded(e);

        _index = _childIndexProvider.GetChildIndex(this);
        _delay = TimeSpan.FromMilliseconds(_index * 15);

        Opacity = 0;
        await Task.Delay(200);
        RunAnimation();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
        base.OnApplyTemplate(e);

        if (Parent is ListBox lb)
            _childIndexProvider = lb;
        else if (Parent?.Parent?.Parent is ListBox lb1)
            _childIndexProvider = lb1;
        else
            throw new InvalidOperationException("Tile must be a child of ItemsRepeater");

        if (!_childIndexProvider.TryGetTotalCount(out _totalCount))
            throw new InvalidOperationException("ItemsRepeater must have a data source");
    }
}