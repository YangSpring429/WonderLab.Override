using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using System;

namespace WonderLab.Media.Behaviors;

public sealed class Parallax3dEffectBehavior : Behavior<Control> {
    public static Rotate3DTransform CreateAnimationRotate3DTransform(TimeSpan duration = default) {
        return new Rotate3DTransform() {
            Depth = 300,
            Transitions = [
                new DoubleTransition {
                    Property = Rotate3DTransform.AngleXProperty,
                    Duration = duration,
                },
                new DoubleTransition {
                    Property = Rotate3DTransform.AngleYProperty,
                    Duration = duration,
                },
                new DoubleTransition {
                    Property = Rotate3DTransform.CenterXProperty,
                    Duration = duration,
                },
                new DoubleTransition {
                    Property = Rotate3DTransform.CenterYProperty,
                    Duration = duration,
                },
            ]
        };
    }

    private Rotate3DTransform GetRotate3DTransform() {
        return (Rotate3DTransform)((TransformGroup)AssociatedObject.RenderTransform).Children[0];
    }

    protected override void OnAttachedToVisualTree() {
        base.OnAttachedToVisualTree();

        AssociatedObject.UseLayoutRounding = false;
        AssociatedObject.RenderTransform = new TransformGroup {
            Children = {
                CreateAnimationRotate3DTransform(TimeSpan.FromSeconds(0.15)),
                new ScaleTransform()
            }
        };

        AssociatedObject.PointerMoved += OnControlPointerMoved;
        AssociatedObject.PointerExited += OnControlPointerExited;
    }

    protected override void OnDetachedFromVisualTree() {
        base.OnDetachedFromVisualTree();

        AssociatedObject.PointerMoved -= OnControlPointerMoved;
        AssociatedObject.PointerExited -= OnControlPointerExited;
    }

    private void OnControlPointerMoved(object sender, PointerEventArgs e) {
        var pos = e.GetPosition(AssociatedObject);

        double multiple = 5.0;

        double centerX = AssociatedObject.Bounds.Width / 2.0;
        double centerY = AssociatedObject.Bounds.Height / 2.0;

        double calcX = -(pos.Y - centerY) / multiple;
        double calcY = (pos.X - centerX) / multiple;

        var rotate3DTransform = GetRotate3DTransform();
        rotate3DTransform.CenterX = calcX;
        rotate3DTransform.CenterY = calcY;
        rotate3DTransform.AngleX = calcX / 2.0;
        rotate3DTransform.AngleY = calcY / 2.0;
    }

    private void OnControlPointerExited(object sender, PointerEventArgs e) {
        // 获取鼠标全局坐标
        var pointerScreen = e.GetPosition(null);
        // 获取控件左上角的屏幕坐标
        var controlScreen = AssociatedObject.PointToScreen(new Point(0, 0));
        var bounds = AssociatedObject.Bounds;

        // 计算鼠标是否真的在控件外
        var inX = pointerScreen.X >= controlScreen.X && pointerScreen.X <= controlScreen.X + bounds.Width;
        var inY = pointerScreen.Y >= controlScreen.Y && pointerScreen.Y <= controlScreen.Y + bounds.Height;

        if (inX && inY) {
            // 鼠标还在控件内，不复位
            return;
        }

        var rotate3DTransform = GetRotate3DTransform();

        rotate3DTransform.CenterX = AssociatedObject.Bounds.Center.X;
        rotate3DTransform.CenterY = AssociatedObject.Bounds.Center.Y;
        rotate3DTransform.AngleX = 0.0;
        rotate3DTransform.AngleY = 0.0;
    }
}