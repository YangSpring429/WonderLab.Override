using System;
using System.Threading.Tasks;
using WonderLab.Classes.Interfaces;

namespace WonderLab.Classes.Models;

public record struct TaskProgress(double Progress, TaskStatus TaskStatus,
    Exception Exception = default, double? Speed = default, bool IsSupportSpeed = false) : ITaskProgress;