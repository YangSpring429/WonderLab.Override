using Avalonia.Threading;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WonderLab.Infrastructure.Interfaces;
using WonderLab.Infrastructure.Models;

namespace WonderLab.Services;

public sealed class TaskService {
    private readonly ObservableCollection<TaskModel> _tasks;
    private readonly ILogger<TaskService> _logger;

    public ReadOnlyObservableCollection<TaskModel> Tasks { get; }

    public TaskService(ILogger<TaskService> logger) {
        _tasks = [];
        _logger = logger;
        Tasks = new(_tasks);
    }

    public void QueueJob(ITaskJob<TaskProgress> job) => Task.Run(async () => {
        job.Completed += (_, _) => {
            _tasks.Remove(new(job));
        };

        await Dispatcher.UIThread.InvokeAsync(() => {
            _tasks.Add(new(job));
            _logger.LogInformation("The job name is {jobName}", job.JobName);
        }, DispatcherPriority.Background);
    });
}