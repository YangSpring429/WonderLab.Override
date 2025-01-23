using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using System;

namespace WonderLab.Controls;

[PseudoClasses(":press", ":panelopen", ":panelclose", ":panelhide", ":panelshow", ":panelhideopen", ":panelhideclose")]
public sealed class AutoPanelViewer : ContentControl {
    private bool _isPress;
    private double _startX;
    private double _offsetX;
    private bool _oldOPenPanelPropertyValue;
    private bool _canOpenPanel;
    private Border _PART_LayoutBorder;

    public enum AutoPanelState {
        Expanded,
        Collapsed,
        Hidden
    }

    //public static readonly StyledProperty<bool> IsOpenPanelProperty =
    //    AvaloniaProperty.Register<AutoPanelViewer, bool>(nameof(IsOpenPanel));

    //public static readonly StyledProperty<bool> IsHidePanelProperty =
    //    AvaloniaProperty.Register<AutoPanelViewer, bool>(nameof(IsHidePanel));

    public static readonly StyledProperty<AutoPanelState> PanelStateProperty =
        AvaloniaProperty.Register<AutoPanelViewer, AutoPanelState>(nameof(PanelState), AutoPanelState.Collapsed);

    public static readonly StyledProperty<double> PanelWidthProperty =
        AvaloniaProperty.Register<AutoPanelViewer, double>(nameof(PanelWidth));

    public static readonly StyledProperty<double> PanelHeightProperty =
        AvaloniaProperty.Register<AutoPanelViewer, double>(nameof(PanelHeight));

    //public bool IsOpenPanel {
    //    get => GetValue(IsOpenPanelProperty);
    //    set => SetValue(IsOpenPanelProperty, value);
    //}

    //public bool IsHidePanel {
    //    get => GetValue(IsHidePanelProperty);
    //    set => SetValue(IsHidePanelProperty, value);
    //}

    //public bool IsHideOpenPanel {
    //    get => GetValue(IsHideOpenPanelProperty);
    //    set => SetValue(IsHideOpenPanelProperty, value);
    //}

    public AutoPanelState PanelState {
        get => GetValue(PanelStateProperty);
        set => SetValue(PanelStateProperty, value);
    }

    public double PanelWidth {
        get => GetValue(PanelWidthProperty);
        set => SetValue(PanelWidthProperty, value);
    }

    public double PanelHeight {
        get => GetValue(PanelHeightProperty);
        set => SetValue(PanelHeightProperty, value);
    }

    private void SetPseudoclasses(bool isPress, bool isPanelOpen, bool isPanelClose, bool isPanelHide, bool isPanelShow, bool isPanelHideOpen, bool isPanelHideClose) {
        PseudoClasses.Set(":press", isPress);
        PseudoClasses.Set(":panelopen", isPanelOpen);
        PseudoClasses.Set(":panelclose", isPanelClose);
        PseudoClasses.Set(":panelhide", isPanelHide);
        PseudoClasses.Set(":panelshow", isPanelShow);
        PseudoClasses.Set(":panelhideopen", isPanelHideOpen);
        PseudoClasses.Set(":panelhideclose", isPanelHideClose);
    }

    private void OnLayoutPointerMoved(object sender, PointerEventArgs e) {
        if (PanelState is not AutoPanelState.Collapsed) {
            return;
        }

        if (e.GetCurrentPoint(_PART_LayoutBorder).Properties.IsLeftButtonPressed) {
            var position = e.GetPosition(this);
            _offsetX = position.X - _startX;
            if (_offsetX > 0 || _offsetX < -15) {
                return;
            }

            _canOpenPanel = _offsetX < -5;
            _PART_LayoutBorder.Margin = new(0, 0, -_offsetX, 0);
        }
    }

    private void OnLayoutPointerReleased(object sender, PointerReleasedEventArgs e) {
        if (PanelState is not AutoPanelState.Collapsed) {
            return;
        }

        SetPseudoclasses(_isPress = false, false, false, false, false, false, false);
        if (e.InitialPressMouseButton is MouseButton.Left) {
            _PART_LayoutBorder.Margin = new Thickness(0, 0, 0, 0);

            if (_offsetX is 0) {
                PanelState = AutoPanelState.Expanded;
            }

            if (_canOpenPanel) {
                PanelState = _canOpenPanel ? AutoPanelState.Expanded : AutoPanelState.Collapsed;
                _canOpenPanel = false;
            }

            _offsetX = 0;
        }
    }

    private void OnLayoutPointerPressed(object sender, PointerPressedEventArgs e) {
        if (PanelState is not AutoPanelState.Collapsed) {
            return;
        }

        SetPseudoclasses(_isPress = true, false, false, false, false, false, false);
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
        var panelStateObservable = this.GetObservable(PanelStateProperty);
        var boundsObservable = (_PART_LayoutBorder.Parent as Visual).GetObservable(BoundsProperty);
        IDisposable disposable = default;

        panelStateObservable.Subscribe(value => {
            if (value is AutoPanelState.Expanded) {
                disposable = boundsObservable.Subscribe(x1 => {
                    _PART_LayoutBorder.Height = x1.Height;
                });
            } else {
                disposable?.Dispose();
            }
        });
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
        base.OnPropertyChanged(change);

        if (change.Property == PanelStateProperty) {
            var (oldValue, newValue) = change.GetOldAndNewValue<AutoPanelState>();

            switch ((oldValue, newValue)) {
                case (AutoPanelState.Collapsed, AutoPanelState.Expanded):
                    SetPseudoclasses(_isPress, true, false, false, false, false, false);
                    break;
                case (AutoPanelState.Expanded, AutoPanelState.Collapsed):
                    SetPseudoclasses(_isPress, false, true, false, false, false, false);
                    break;
                case (AutoPanelState.Collapsed, AutoPanelState.Hidden):
                    SetPseudoclasses(_isPress, false, false, true, false, false, false);
                    break;
                case (AutoPanelState.Hidden, AutoPanelState.Collapsed):
                    SetPseudoclasses(_isPress, false, false, false, true, false, false);
                    break;
                case (AutoPanelState.Expanded, AutoPanelState.Hidden):
                    SetPseudoclasses(_isPress, false, false, false, false, false, true);
                    break;
                case (AutoPanelState.Hidden, AutoPanelState.Expanded):
                    SetPseudoclasses(_isPress, false, false, false, false, true, false);
                    break;
            }


            //if (change.Property == IsOpenPanelProperty) {
            //    //if (IsOpenPanel && IsHidePanel) {
            //    //    IsHidePanel = false;
            //    //    SetPseudoclasses(_isPress, false, false, false, false, true, false);
            //    //    return;
            //    //}else if (!IsOpenPanel && IsHidePanel) {
            //    //    return;
            //    //}

            //    SetPseudoclasses(_isPress, IsOpenPanel, !IsOpenPanel, false, false, false, false);
            //}

            //if (change.Property == IsHidePanelProperty) {
            //    //if (IsHidePanel && IsOpenPanel) {
            //    //    SetPseudoclasses(_isPress, false, false, false, false, false, true);
            //    //    IsOpenPanel = false;
            //    //    return;
            //    //}


            //    SetPseudoclasses(_isPress, false, false, IsHidePanel, !IsHidePanel, false, false);
            //}


        }
    }
}