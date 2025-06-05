using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using System;
using System.Diagnostics;

namespace WonderLab.Controls;

[Obsolete("由于渲染 Bug 暂无有效解决方法弃用")]
public sealed class BlurBorder : ContentControl {
    private static readonly ExperimentalAcrylicMaterial DefaultAcrylicMaterialDark = new() {
        MaterialOpacity = 0.65,
        TintColor = Colors.Black,
        TintOpacity = 0.7,
        BackgroundSource = AcrylicBackgroundSource.Digger,
    };

    private static readonly ExperimentalAcrylicMaterial DefaultAcrylicMaterialLight = new() {
        MaterialOpacity = 0.65,
        TintColor = Colors.White,
        TintOpacity = 0.7,
        BackgroundSource = AcrylicBackgroundSource.Digger,
    };

    public static readonly StyledProperty<ExperimentalAcrylicMaterial> MaterialProperty =
        AvaloniaProperty.Register<BlurBorder, ExperimentalAcrylicMaterial>(nameof(Material), DefaultAcrylicMaterialDark);

    public static readonly StyledProperty<float> BlurRadiusProperty =
        AvaloniaProperty.Register<BlurBorder, float>(nameof(BlurRadius), 30);

    public float BlurRadius {
        get => GetValue(BlurRadiusProperty);
        set => SetValue(BlurRadiusProperty, value);
    }

    public ExperimentalAcrylicMaterial Material {
        get => GetValue(MaterialProperty);
        set => SetValue(MaterialProperty, value);
    }

    static BlurBorder() {
        AffectsRender<BlurBorder>(BlurRadiusProperty);
    }
}

[Obsolete("由于渲染 Bug 暂无有效解决方法弃用")]
internal sealed class BlurBackground : Decorator {
    public static readonly StyledProperty<float> BlurRadiusProperty =
        AvaloniaProperty.Register<BlurBorder, float>(nameof(BlurRadius), 5);

    public float BlurRadius {
        get => GetValue(BlurRadiusProperty);
        set => SetValue(BlurRadiusProperty, value);
    }

    public override void Render(DrawingContext context) {
        base.Render(context);
        context.Custom(new BlurBackgroundOperation(new Rect(default, Bounds.Size), BlurRadius));
    }

    public BlurBackground() {
        AffectsRender<BlurBackground>(BlurRadiusProperty);

        //DispatcherTimer.Run(() => {
        //    var topLevel = TopLevel.GetTopLevel(this) as Window;
        //if (topLevel.IsActive && topLevel.IsPointerOver) {
        //        InvalidateVisual();
        //    }
        //    return true;
        //}, TimeSpan.FromMilliseconds(10), DispatcherPriority.Render);
    }

    private class BlurBackgroundOperation : ICustomDrawOperation {
        private readonly Rect _bounds;
        private readonly float _radius;

        public BlurBackgroundOperation(Rect bounds, float radius) {
            _bounds = bounds;
            _radius = radius;
        }

        public void Dispose() { }

        public bool HitTest(Point p) => _bounds.Contains(p);

        public void Render(ImmediateDrawingContext context) {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            using var lease = leaseFeature.Lease();
            var canvas = lease.SkCanvas;

            if (!canvas.TotalMatrix.TryInvert(out var currentInvertedTransform))
                return;

            using var backgroundSnapshot = lease.SkSurface.Snapshot();
            using var backdropShader = SKShader.CreateImage(backgroundSnapshot, SKShaderTileMode.Clamp,
                SKShaderTileMode.Clamp, currentInvertedTransform);

            using var blurred = SKSurface.Create(lease.GrContext, false,
                new SKImageInfo((int)Math.Ceiling(_bounds.Width),
                    (int)Math.Ceiling(_bounds.Height),
                        SKImageInfo.PlatformColorType, SKAlphaType.Premul));

            using var filter = SKImageFilter.CreateBlur(_radius, _radius);

            using var blurPaint = new SKPaint {
                Shader = backdropShader,
                ImageFilter = filter
            };

            blurred.Canvas.Clear();
            blurred.Canvas.DrawRect(0, 0, (float)_bounds.Width, (float)_bounds.Height, blurPaint);

            using var blurSnap = blurred.Snapshot();
            using var blurSnapShader = SKShader.CreateImage(blurSnap);
            using var blurSnapPaint = new SKPaint {
                Shader = blurSnapShader,
                IsAntialias = false,
            };

            Debug.WriteLine("render!");
            canvas.DrawRect(0, 0, (float)_bounds.Width, (float)_bounds.Height, blurSnapPaint);
        }

        public Rect Bounds => _bounds;

        public bool Equals(ICustomDrawOperation other) {
            return other is BlurBackgroundOperation op && op._bounds == _bounds;
        }
    }
}