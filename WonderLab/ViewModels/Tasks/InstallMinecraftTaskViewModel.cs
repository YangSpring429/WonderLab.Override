using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftLaunch.Components.Installer;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Interfaces;
using WonderLab.Infrastructure.Models;

namespace WonderLab.ViewModels.Tasks;

public sealed partial class InstallMinecraftTaskViewModel : ObservableObject, ITaskJob<TaskProgress> {
    public readonly CancellationTokenSource InstallCancellationTokenSource = new();

    public double MaxProgress => 1d;
    public string ProgressText => Progress.ToString("P2");

    public ImmutableArray<TaskStep> TaskSteps { get; } = [
        new TaskStep { StepName = "Check if the specified id exists", TaskStatus = TaskStatus.Running },
        new TaskStep { StepName = "Download minecraft" },
        new TaskStep { StepName = "Install mod loader" },
    ];

    public event EventHandler Completed;

    public Exception Exception { get; set; }
    public required string JobName { get; set; }

    [ObservableProperty] private TaskStatus _taskStatus;
    [ObservableProperty] private bool _isIndeterminate = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ProgressText))]
    private double _progress;

    public CancellationToken TaskCancellationToken => InstallCancellationTokenSource.Token;

    public void Report(TaskProgress value) => Dispatcher.UIThread.InvokeAsync(() => {
        switch (value.Step) {
            case 1:
                TaskStatus = TaskStatus.Running;
                IsIndeterminate = false;
                TaskSteps[0].Progress = value.Progress;
                break;
            case 2:
                TaskSteps[0].TaskStatus = TaskStatus.RanToCompletion;
                TaskSteps[1].TaskStatus = TaskStatus.Running;

                TaskSteps[1].Speed = value.Speed;
                TaskSteps[1].Progress = value.Progress;
                break;
            case 3:
                TaskSteps[1].TaskStatus = TaskStatus.RanToCompletion;
                TaskSteps[2].TaskStatus = TaskStatus.Running;

                TaskSteps[2].Speed = value.Speed;
                TaskSteps[2].Progress = value.Progress;
                break;
            case 4:
                Completed?.Invoke(this, EventArgs.Empty);
                break;
        }

        Progress = TaskSteps.Select(x => x.Progress).Sum() / (double)TaskSteps.Length;
    }, DispatcherPriority.ApplicationIdle);

    [RelayCommand]
    private void CancelTask() {
        InstallCancellationTokenSource.Cancel();
    }
}