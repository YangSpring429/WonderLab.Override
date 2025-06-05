using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace WonderLab.Controls.Experimental.BreadcrumbBar;

[PseudoClasses(":last", ":ellipsis", ":ellipsis-dropdown")]
public class BreadcrumbBarItem : ContentControl {
    private int _itemIndex;
    private Button _button;
    private BreadcrumbBar _parent;

    internal bool CreatedByBreadcrumbElementFactory { get; set; }
    internal bool IsEllipsisButton => PseudoClasses.Contains(":ellipsis");
    internal bool IsEllipsisDropDownItem => PseudoClasses.Contains(":ellipsis-dropdown");

    internal void SetParentBreadcrumb(BreadcrumbBar parent) =>
        _parent = parent;

    internal void SetEllipsis(bool value) =>
        PseudoClasses.Set(":ellipsis", value);

    internal void SetLast(bool value) =>
        PseudoClasses.Set(":last", value);

    internal void SetIsEllipsisDropDownItem(bool value) =>
        PseudoClasses.Set(":ellipsis-dropdown", value);

    internal void SetIndex(int itemIndex) =>
        _itemIndex = itemIndex;

    private void OnButtonClick(object sender, RoutedEventArgs e) {
        if (_parent != null) {
            if (IsEllipsisButton)
                _parent.OpenFlyout();
            else if (IsEllipsisDropDownItem) {
                _parent.CloseFlyout();
                RaiseItemClickedEvent(Content, _itemIndex);
            } else
                RaiseItemClickedEvent(Content, _itemIndex);
        }
    }

    private void RaiseItemClickedEvent(object content, int index) {
        content = CreatedByBreadcrumbElementFactory ? content : DataContext;
        _parent?.RaiseItemClickedEvent(content, index);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
        base.OnApplyTemplate(e);
        _button = e.NameScope.Find<Button>("PART_ItemButton");
        if (_button is not null)
            _button.Click += OnButtonClick;
    }
}