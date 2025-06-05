using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WonderLab.Classes.Interfaces;

public interface ITaskJob<in T> : IProgress<T> {
    string JobName { get; }
    double? Speed { get; }
    string SpeedText { get; }
    string ProgressText { get; }
    double Progress { get; set; }
    bool IsSupportSpeed { get; set; }
    bool IsIndeterminate { get; set; }
    string RunningTimeText { get; set; }
    Exception Exception { get; }
    TaskStatus TaskStatus { get; set; }
    IRelayCommand CancelTaskCommand { get; }
    CancellationToken TaskCancellationToken { get; }

    event EventHandler Completed;
}