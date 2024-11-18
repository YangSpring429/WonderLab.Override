using System.Runtime.InteropServices;
using System;
using System.Runtime.Versioning;

namespace WonderLab.Platform.Linux.X11;

[SupportedOSPlatform("Linux")]
public static class WindowHandler {
    public const string X11Library = "libX11.so";

    public static void HideTitleBar(IntPtr windowHandle) {
        IntPtr display = XOpenDisplay(IntPtr.Zero);
        if (display == IntPtr.Zero) {
            throw new ArgumentNullException();
        }

        //get window atom
        IntPtr mwmHintsProperty = XInternAtom(display, "_MOTIF_WM_HINTS", false);
        if (mwmHintsProperty == IntPtr.Zero) {
            throw new ArgumentNullException();
        }

        MotifWmHints hints = new MotifWmHints {
            Flags = (IntPtr)(HintsFlags.Functions | HintsFlags.Decorations),
            Functions = IntPtr.Zero,
            Decorations = IntPtr.Zero,
            InputMode = IntPtr.Zero,
            Status = IntPtr.Zero,
        };

        IntPtr hintsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(hints));
        Marshal.StructureToPtr(hints, hintsPtr, false);
        _ = XChangeProperty(display, windowHandle, mwmHintsProperty, mwmHintsProperty, 32, PropertyMode.Replace, hintsPtr, 5);
        Marshal.FreeHGlobal(hintsPtr);

        XFlush(display);
        XCloseDisplay(display);
    }

    [DllImport(X11Library)]
    public static extern IntPtr XOpenDisplay(IntPtr display);

    [DllImport(X11Library)]
    public static extern int XCloseDisplay(IntPtr display);

    [DllImport(X11Library)]
    public static extern IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

    [DllImport(X11Library)]
    public static extern int XChangeProperty(IntPtr display, IntPtr w, IntPtr property, IntPtr type, int format, PropertyMode mode, IntPtr data, int nelements);

    [DllImport(X11Library)]
    public static extern int XFlush(IntPtr display);
}

[StructLayout(LayoutKind.Sequential)]
public struct MotifWmHints {
    public IntPtr Flags;
    public IntPtr Functions;
    public IntPtr Decorations;
    public IntPtr InputMode;
    public IntPtr Status;
}

[Flags]
public enum HintsFlags : long {
    Functions = 1L << 0,
    Decorations = 1L << 1,
    InputMode = 1L << 2,
    Status = 1L << 3
}

public enum PropertyMode {
    Replace = 0,
    Prepend = 1,
    Append = 2
}