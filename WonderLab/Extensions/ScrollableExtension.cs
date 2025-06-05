using Avalonia.Animation.Easings;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using System;

namespace WonderLab.Extensions;

public static class ScrollableExtension {
    public static void ApplyScrollAnimated(CompositionVisual cv) {
        if (cv == null)
            return;

        Compositor compositor = cv.Compositor;
        var animationGroup = compositor.CreateAnimationGroup();
        Vector3KeyFrameAnimation offsetAnimation = compositor.CreateVector3KeyFrameAnimation();

        offsetAnimation.Target = "Offset";
        offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue", new ExponentialEaseOut());
        offsetAnimation.Duration = TimeSpan.FromMilliseconds(250);

        ImplicitAnimationCollection implicitAnimationCollection = compositor.CreateImplicitAnimationCollection();
        animationGroup.Add(offsetAnimation);
        implicitAnimationCollection["Offset"] = animationGroup;

        cv.ImplicitAnimations = implicitAnimationCollection;
    }
}