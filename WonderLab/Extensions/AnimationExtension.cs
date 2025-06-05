using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Styling;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WonderLab.Extensions;

public static class FluentAnimatorExtensions {
    public static FluentAnimator<T> Animate<T>(this Animatable control, AvaloniaProperty<T> property) {
        return new FluentAnimator<T>(control, property);
    }
}

public class FluentAnimator<T> {
    private T _from;
    private T _to;
    private readonly Animatable _control;
    private readonly AvaloniaProperty<T> _property;
    private Easing _easing = new ExponentialEaseOut();
    private TimeSpan _duration = TimeSpan.FromMilliseconds(500);

    public FluentAnimator(Animatable control, AvaloniaProperty<T> property) {
        _control = control;
        _property = property;
    }

    public FluentAnimator<T> From(T value) {
        _from = value;
        return this;
    }

    public FluentAnimator<T> To(T value) {
        _to = value;
        return this;
    }

    public FluentAnimator<T> WithDuration(TimeSpan duration) {
        _duration = duration;
        return this;
    }

    public FluentAnimator<T> WithEasing(Easing easing) {
        _easing = easing;
        return this;
    }

    public Task RunAsync(CancellationToken cancellationToken = default) {
        if (cancellationToken is { IsCancellationRequested: true }) {
            return Task.CompletedTask;
        }

        return new Animation {
            Duration = _duration,
            FillMode = FillMode.Both,
            Easing = _easing,
            Children = {
                new KeyFrame {
                    Setters = { new Setter { Property = _property, Value = _from } },
                    KeyTime = TimeSpan.Zero
                },
                new KeyFrame {
                    Setters = { new Setter { Property = _property, Value = _to } },
                    KeyTime = _duration
                }
            }
        }.RunAsync(_control, cancellationToken);
    }
}