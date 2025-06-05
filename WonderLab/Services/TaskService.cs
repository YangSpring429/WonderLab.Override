using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WonderLab.Classes.Interfaces;
using WonderLab.Classes.Models;

namespace WonderLab.Services;

public sealed class TaskService {
    private readonly ILogger<TaskService> _logger;

    public ObservableCollection<TaskModel> Tasks { get; }

    public TaskService(ILogger<TaskService> logger) {
        Tasks = [];
        _logger = logger;
    }

    public void QueueJob(ITaskJob<TaskProgress> job) => Task.Run(async () => {
        job.Completed += (_, _) => {
            Tasks.Remove(new(job));
        };

        await Dispatcher.UIThread.InvokeAsync(() => {
            Tasks.Add(new(job));
            _logger.LogInformation("已将 {jobName} 添加至调度队列", job.JobName);
        }, DispatcherPriority.Background);
    });
}