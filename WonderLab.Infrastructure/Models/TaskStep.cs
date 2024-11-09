using CommunityToolkit.Mvvm.ComponentModel;

namespace WonderLab.Infrastructure.Models;

public sealed partial class TaskStep : ObservableObject {
    [ObservableProperty] private string _stepName;
    [ObservableProperty] private double _maxProgress = 1d;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressIcon))]
    private TaskStatus _taskStatus = TaskStatus.WaitingToRun;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressText))]
    private double _progress;

    public string ProgressText => Progress.ToString("P2");
    public string ProgressIcon => TaskStatus is TaskStatus.RanToCompletion ? "\uE73E" : "\uE712";
}