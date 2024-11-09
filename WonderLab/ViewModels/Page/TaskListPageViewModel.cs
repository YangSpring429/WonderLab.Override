using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WonderLab.Infrastructure.Models;
using WonderLab.Services;

namespace WonderLab.ViewModels.Page;

public sealed partial class TaskListPageViewModel : ObservableObject {
    private readonly TaskService _taskService;

    public bool HasTask => Tasks.Count > 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTask))]
    private ReadOnlyObservableCollection<TaskModel> _tasks;

    public TaskListPageViewModel(TaskService taskService) {
        _taskService = taskService;
        Tasks = _taskService.Tasks;
    }
}