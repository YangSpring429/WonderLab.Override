using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace WonderLab.Controls.Media.Transitions;

public sealed class EntrancePageTransition : IPageTransition {
    public TimeSpan Duration { get; set; }
    public Easing Easing { get; set; } = new SplineEasing(0.1, 0.9, 0.2, 1.0);

    public EntrancePageTransition() { }

    public EntrancePageTransition(TimeSpan duration = default) {
        Duration = duration;
    }

    //public async Task Start(Visual from, Visual to, bool forward, CancellationToken cancellationToken = default) {
    //    if (cancellationToken.IsCancellationRequested) {
    //        return;
    //    }

    //    if (from is not null) {
    //        var fromComposition = ElementComposition.GetElementVisual(from);
    //        var fromCompositor = fromComposition.Compositor;

    //        var opacityAni = fromCompositor.CreateScalarKeyFrameAnimation();
    //        opacityAni.Duration = Duration;
    //        opacityAni.Target = "Opacity";

    //        opacityAni.InsertKeyFrame(0f, 1f);
    //        opacityAni.InsertKeyFrame(1f, 0f, Easing);

    //        fromComposition.StartAnimation("Opacity", opacityAni);
    //    }

    //    if (to is not null) {
    //        var toComposition = ElementComposition.GetElementVisual(to);
    //        var toCompositor = toComposition.Compositor;

    //        var opacityAni = toCompositor.CreateScalarKeyFrameAnimation();
    //        opacityAni.Duration = Duration;
    //        opacityAni.Target = "Opacity";

    //        opacityAni.InsertKeyFrame(0f, 0f);
    //        opacityAni.InsertKeyFrame(1f, 1f, Easing);

    //        var offsetAni = toCompositor.CreateVector3KeyFrameAnimation();
    //        offsetAni.Duration = Duration;
    //        offsetAni.Target = "Offset";

    //        offsetAni.InsertKeyFrame(0f, new(0, 130, 0));
    //        offsetAni.InsertKeyFrame(1f, Vector3.Zero, Easing);

    //        var aniGroup = toCompositor.CreateAnimationGroup();
    //        aniGroup.Add(opacityAni);
    //        aniGroup.Add(offsetAni);

    //        toComposition.StartAnimationGroup(aniGroup);
    //    }
    //}

    public async Task Start(Visual from, Visual to, bool forward, CancellationToken cancellationToken) {
        if (cancellationToken.IsCancellationRequested) {
            return;
        }

        var tasks = new List<Task>();

        if (from is not null) {
            var animation = new Animation {
                Easing = Easing,
                Duration = Duration,
                FillMode = FillMode.Forward,
                Children = {
                    new KeyFrame {
                        Cue = new(0d),
                        Setters = {
                            new Setter {
                                Value = 1d,
                                Property = Visual.OpacityProperty
                            },
                        }
                    },

                    new KeyFrame {
                        Cue= new(1d),
                        Setters = {
                            new Setter {
                                Value = 0d,
                                Property = Visual.OpacityProperty
                            },
                        }
                    }
                }
            };

            tasks.Add(animation.RunAsync(from, cancellationToken));
        }

        if (to is not null) {
            var animation = new Animation {
                Easing = Easing,
                Duration = Duration,
                FillMode = FillMode.Forward,
                Children = {
                    new KeyFrame {
                        Cue = new(0d),
                        Setters = {
                            new Setter {
                                Value = 0d,
                                Property = Visual.OpacityProperty
                            },
                            new Setter {
                                Value = 130d,
                                Property = TranslateTransform.YProperty
                            },
                        }
                    },

                    new KeyFrame {
                        Cue= new(1d),
                        Setters = {
                            new Setter {
                                Value = 1d,
                                Property = Visual.OpacityProperty
                            },
                            new Setter {
                                Value = 0d,
                                Property = TranslateTransform.YProperty
                            },
                        }
                    }
                }
            };

            tasks.Add(animation.RunAsync(to, cancellationToken));
        }

        await Task.WhenAll(tasks);

        (from as Control).IsHitTestVisible = false;
        (to as Control).IsHitTestVisible = true;
    }
}