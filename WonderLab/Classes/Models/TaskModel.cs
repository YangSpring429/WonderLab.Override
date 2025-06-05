using WonderLab.Classes.Interfaces;

namespace WonderLab.Classes.Models;

public sealed record TaskModel(ITaskJob<TaskProgress> TaskJob);