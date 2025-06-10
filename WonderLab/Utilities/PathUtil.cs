using MinecraftLaunch.Base.Utilities;
using System;
using System.IO;

namespace WonderLab.Utilities;

public static class PathUtil {
    public static string DefaultDirectory => EnvironmentUtil.IsMac
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "WonderLab")
        : Path.Combine(Environment.CurrentDirectory, "WonderLab");
}