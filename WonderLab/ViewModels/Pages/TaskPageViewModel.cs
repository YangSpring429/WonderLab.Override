using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WonderLab.Classes.Models;
using WonderLab.Services;

namespace WonderLab.ViewModels.Pages;

public sealed partial class TaskPageViewModel : DynamicPageViewModelBase {
    private readonly TaskService _taskService;
    private readonly ILogger<TaskPageViewModel> _logger;

    [ObservableProperty] private bool _hasTasks;

    public ReadOnlyObservableCollection<TaskModel> Tasks { get; }

    public TaskPageViewModel(TaskService taskService, ILogger<TaskPageViewModel> logger) {
        _logger = logger;
        Tasks = new(taskService.Tasks);

        HasTasks = Tasks.Count is > 0;
        taskService.Tasks.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
#if DEBUG
        _logger.LogInformation("Task Count is {count}", Tasks.Count);
#endif

        HasTasks = Tasks.Count is > 0;
    }
}