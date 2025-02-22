using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WonderLab.Controls;
using WonderLab.Extensions.Hosting.UI;
using WonderLab.Infrastructure.Models;
using WonderLab.Infrastructure.Models.Launch;
using WonderLab.Infrastructure.Models.Messaging;
using WonderLab.Services;
using WonderLab.Services.Launch;

namespace WonderLab.ViewModels.Window;

public sealed partial class MainWindowViewModel : ObservableObject {
    private readonly TaskService _taskService;

    public double BackgroundOpacity => ActivePageIndex is 0 ? 0 : 0.45;

    public AvaloniaPageProvider PageProvider { get; }
    public ReadOnlyObservableCollection<TaskModel> Tasks { get; }
    public ReadOnlyObservableCollection<GameProcess> GameProcesses { get; }

    [ObservableProperty] private AutoPanelViewer.AutoPanelState _panelState = AutoPanelViewer.AutoPanelState.Collapsed;
    [ObservableProperty] private string _assistantPanelPageKey;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundOpacity))]
    private int _activePageIndex = -1;

    [ObservableProperty]
    private string _pageKey;

    public MainWindowViewModel(AvaloniaPageProvider avaloniaPageProvider, TaskService taskService, LaunchService launchService) {
        try {
            _taskService = taskService;
            PageProvider = avaloniaPageProvider;
            Tasks = _taskService.Tasks;
            GameProcesses = new(launchService.GameProcesses);

            App.Get<GameService>().RefreshGames();
        } catch (Exception) {}

        WeakReferenceMessenger.Default.Register<PageNotificationMessage>(this, (_, arg) => {
            ActivePageIndex = arg.PageKey is "Home" ? 0 : -1;
            PageKey = arg.PageKey;

            PanelState = ActivePageIndex is -1
                ? AutoPanelViewer.AutoPanelState.Hidden
                : AutoPanelViewer.AutoPanelState.Collapsed;
        });

        WeakReferenceMessenger.Default.Register<PanelPageNotificationMessage>(this, (_, arg) => {
            if (arg.PageKey is "close") {
                PanelState = AutoPanelViewer.AutoPanelState.Collapsed;
                return;
            }

            if (arg.PageKey is "GameSetting/Navigation") {
                PanelState = AutoPanelViewer.AutoPanelState.Expanded;
            }

            AssistantPanelPageKey = arg.PageKey;
        });
    }

    [RelayCommand]
    private void OnLoaded() {
        ActivePageIndex = 0;
    }

    [RelayCommand]
    private void GoToTaskList() {
        ActivePageIndex = -1;
        PageKey = "TaskList";
        PanelState = AutoPanelViewer.AutoPanelState.Hidden;
    }

    [RelayCommand]
    private void ChangeActivePage() {
        PageKey = ActivePageIndex switch {
            0 => "Home",
            1 => "Download/Navigation",
            2 => "Multiplayer",
            3 => "Setting/Navigation",
            _ => PageKey ?? "Home",
        };

        PanelState = ActivePageIndex switch {
            0 => AutoPanelViewer.AutoPanelState.Collapsed,
            1 => AutoPanelViewer.AutoPanelState.Hidden,
            2 => AutoPanelViewer.AutoPanelState.Hidden,
            3 => AutoPanelViewer.AutoPanelState.Hidden,
            _ => PanelState
        };
    }

    [RelayCommand]
    private void KillGameProcess(GameProcess gameProcess) {
        //gameProcess.ProcessWatcher.Process.Kill();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(PanelState) && PanelState is AutoPanelViewer.AutoPanelState.Expanded) {
            if (PageKey is "Home") {
                AssistantPanelPageKey = "Dashboard/Home";
            }
        }
    }
}