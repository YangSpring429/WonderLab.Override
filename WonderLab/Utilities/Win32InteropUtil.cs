using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace WonderLab.Utilities;

[SupportedOSPlatform("Windows")]
public static class Win32InteropUtil {
    public static void SetWindowEffect(IntPtr hwnd, int backgroundType = 0) {
        Win32Interop.DwmSetWindowAttribute(hwnd, Win32Interop.DWMWA_SYSTEMBACKDROP_TYPE, ref backgroundType, sizeof(int));
    }
}

internal static partial class Win32Interop {
    internal const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

    [LibraryImport("dwmapi.dll", SetLastError = true)]
    public static partial int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);
}