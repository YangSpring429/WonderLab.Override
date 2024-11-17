using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Avalonia.Xaml.Interactivity;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class WindowTitleBarBehavior : Behavior<Window> {
    protected override void OnAttached() {
        base.OnAttached();
    }

    private void AdjustTitleBarButtonPosition(IntPtr nsWindow) {
        const string AppKitLibrary = "/System/Library/Frameworks/AppKit.framework/AppKit";

        IntPtr GetMethod(string method) => Dlfcn.dlsym(Dlfcn.dlopen(AppKitLibrary, 0), method);

        var selStandardWindowButton = GetMethod("standardWindowButton:");
        var selSetFrameOrigin = GetMethod("setFrameOrigin:");

        var closeButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)1); // NSWindowButtonClose
        var minimizeButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)2); // NSWindowButtonMiniaturize
        var zoomButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)3); // NSWindowButtonZoom

        var newCloseButtonPosition = new CGPoint(20, 20); // 示例位置，请根据需要调整
        var newMinimizeButtonPosition = new CGPoint(40, 20);
        var newZoomButtonPosition = new CGPoint(60, 20);

        objc_msgSend_CGPoint(closeButton, selSetFrameOrigin, newCloseButtonPosition);
        objc_msgSend_CGPoint(minimizeButton, selSetFrameOrigin, newMinimizeButtonPosition);
        objc_msgSend_CGPoint(zoomButton, selSetFrameOrigin, newZoomButtonPosition);
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct CGPoint {
        public double X;
        public double Y;

        public CGPoint(double x, double y) {
            X = x;
            Y = y;
        }
    }

    [DllImport("libobjc.dylib", EntryPoint = "objc_msgSend")] 
    private static extern IntPtr objc_msgSend_IntPtr_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);
    
    [DllImport("libobjc.dylib", EntryPoint = "objc_msgSend")]
    private static extern void objc_msgSend_CGPoint(IntPtr receiver, IntPtr selector, CGPoint arg1);
}

public static class Dlfcn { 
    [DllImport("libSystem.B.dylib", EntryPoint = "dlopen")] 
    public static extern IntPtr dlopen(string path, int mode); 

    [DllImport("libSystem.B.dylib", EntryPoint = "dlsym")] 
    public static extern IntPtr dlsym(IntPtr handle, string symbol); }