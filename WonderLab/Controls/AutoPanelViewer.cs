using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using System;

namespace WonderLab.Controls;

[PseudoClasses(":press", ":panelopen", ":panelclose", ":panelhide", ":panelshow", ":panelhideopen")]
public sealed class AutoPanelViewer : ContentControl {
    private bool _isPress;
    private double _startX;
    private bool _canOpenPanel;
    private Border _PART_LayoutBorder;

    public static readonly StyledProperty<bool> IsOpenPanelProperty =
        AvaloniaProperty.Register<AutoPanelViewer, bool>(nameof(IsOpenPanel));

    public static readonly StyledProperty<bool> IsHidePanelProperty =
        AvaloniaProperty.Register<AutoPanelViewer, bool>(nameof(IsHidePanel));

    public static readonly StyledProperty<double> PanelWidthProperty =
        AvaloniaProperty.Register<AutoPanelViewer, double>(nameof(PanelWidth));

    public static readonly StyledProperty<double> PanelHeightProperty =
        AvaloniaProperty.Register<AutoPanelViewer, double>(nameof(PanelHeight));

    public bool IsOpenPanel {
        get => GetValue(IsOpenPanelProperty);
        set => SetValue(IsOpenPanelProperty, value);
    }

    public bool IsHidePanel {
        get => GetValue(IsHidePanelProperty);
        set => SetValue(IsHidePanelProperty, value);
    }

    public double PanelWidth {
        get => GetValue(PanelWidthProperty);
        set => SetValue(PanelWidthProperty, value);
    }

    public double PanelHeight {
        get => GetValue(PanelHeightProperty);
        set => SetValue(PanelHeightProperty, value);
    }

    private void SetPseudoclasses(bool isPress, bool isPanelOpen, bool isPanelClose, bool isPanelHide, bool isPanelShow, bool isPanelHideOpen) {
        PseudoClasses.Set(":press", isPress);
        PseudoClasses.Set(":panelopen", isPanelOpen);
        PseudoClasses.Set(":panelclose", isPanelClose);
        PseudoClasses.Set(":panelhide", isPanelHide);
        PseudoClasses.Set(":panelshow", isPanelShow);
        PseudoClasses.Set(":panelhideopen", isPanelHideOpen);
    }

    private void OnLayoutPointerMoved(object sender, PointerEventArgs e) {
        if (IsOpenPanel) {
            return;
        }

        if (e.GetCurrentPoint(_PART_LayoutBorder).Properties.IsLeftButtonPressed) {
            var position = e.GetPosition(this);
            var offsetX = position.X - _startX;
            if (offsetX > 0 || offsetX < -15) {
                return;
            }

            _canOpenPanel = offsetX < -5;
            _PART_LayoutBorder.Margin = new(0, 0, -offsetX, 0);
        }
    }

    private void OnLayoutPointerReleased(object sender, PointerReleasedEventArgs e) {
        if (IsOpenPanel) {
            return;
        }

        SetPseudoclasses(_isPress = false, false, false, false, false, false);
        if (e.InitialPressMouseButton is MouseButton.Left) {
            _PART_LayoutBorder.Margin = new Thickness(0, 0, 0, 0);

            if (_canOpenPanel) {
                IsOpenPanel = _canOpenPanel;
                _canOpenPanel = false;
            }
        }
    }

    private void OnLayoutPointerPressed(object sender, PointerPressedEventArgs e) {
        if (IsOpenPanel) {
            return;
        }

        SetPseudoclasses(_isPress = true, false, false, false, false, false);
        if (e.GetCurrentPoint(_PART_LayoutBorder).Properties.IsLeftButtonPressed) {
            _startX = e.GetPosition(this).X;
        }
    }

    private void OnLayoutPointerCaptureLost(object sender, PointerCaptureLostEventArgs e) {
        _PART_LayoutBorder.Margin = new Thickness(0, 0, 0, 0);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
        base.OnApplyTemplate(e);
        _PART_LayoutBorder = e.NameScope.Find<Border>("PART_LayoutBorder");

        _PART_LayoutBorder.PointerMoved += OnLayoutPointerMoved;
        _PART_LayoutBorder.PointerPressed += OnLayoutPointerPressed;
        _PART_LayoutBorder.PointerReleased += OnLayoutPointerReleased;
        _PART_LayoutBorder.PointerCaptureLost += OnLayoutPointerCaptureLost;

        //PropertyChanged Event subscribe.
        var openObservable = this.GetObservable(IsOpenPanelProperty);
        var boundsObservable = (_PART_LayoutBorder.Parent as Visual).GetObservable(BoundsProperty);
        IDisposable disposable = default;

        openObservable.Subscribe(value => {
            if (value) {
                disposable = boundsObservable.Subscribe(x1 => {
                    _PART_LayoutBorder.Height = x1.Height;
                });
            } else {
                disposable?.Dispose();
            }
        });
    }

    protected override async void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
        base.OnPropertyChanged(change);

        if (change.Property == IsOpenPanelProperty) {
            if (IsOpenPanel && IsHidePanel) {
                IsHidePanel = false;
                SetPseudoclasses(_isPress, false, false, false, false, true);
                return;
            }

            SetPseudoclasses(_isPress, IsOpenPanel, !IsOpenPanel, false, false, false);
        }

        if (change.Property == IsHidePanelProperty) {
            //if (IsOpenPanel && IsHidePanel) {
            //    IsOpenPanel = false;
            //    await Task.Delay(TimeSpan.FromSeconds(0.75d));
            //}
            if (!IsHidePanel && IsOpenPanel) {
                return;
            }


            SetPseudoclasses(_isPress, false, false, IsHidePanel, !IsHidePanel, false);
        }
    }
}