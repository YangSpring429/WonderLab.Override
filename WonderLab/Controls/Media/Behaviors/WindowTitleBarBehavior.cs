using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Avalonia.Xaml.Interactivity;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class WindowTitleBarBehavior : Behavior<Window> {
    protected override void OnAttached() {
        base.OnAttached();

        AssociatedObject.Opened+= OnOpened;
        AssociatedObject.PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e) {
        if (e.Property == Window.WindowStateProperty || e.Property == Window.BoundsProperty) {
            var platform = AssociatedObject.TryGetPlatformHandle();
            if (platform is not null) {
                var nsWindow = platform.Handle;
                if (nsWindow != IntPtr.Zero) {
                    try {
                        AdjustTitleBarButtonPosition(nsWindow);
                        HideZoomButton(nsWindow);
                    }
                    catch (Exception exception) {
                        Console.WriteLine(exception);
                        throw;
                    }
                }
            }
        }
    }

    private async void OnOpened(object sender, EventArgs e) {
        var platform = AssociatedObject.TryGetPlatformHandle();
        if (platform is not null) {
            var nsWindow = platform.Handle;
            if (nsWindow != IntPtr.Zero) {
                try {
                    AdjustTitleBarButtonPosition(nsWindow);
                    HideZoomButton(nsWindow);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }

    private void AdjustTitleBarButtonPosition(IntPtr nsWindow) {
        var selStandardWindowButton = sel_registerName("standardWindowButton:");
        var selSetFrameOrigin = sel_registerName("setFrameOrigin:");

        if (selStandardWindowButton == IntPtr.Zero || selSetFrameOrigin == IntPtr.Zero) {
            throw new NullReferenceException();
        }
        
        var closeButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)0); // NSWindowButtonClose
        var minimizeButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)1); // NSWindowButtonMiniaturize
        var zoomButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)2); // NSWindowButtonZoom

        var newCloseButtonPosition = new CGPoint(30, -2);
        var newMinimizeButtonPosition = new CGPoint(50, -2);
        var newZoomButtonPosition = new CGPoint(0, 200);

        objc_msgSend_CGPoint(closeButton, selSetFrameOrigin, newCloseButtonPosition);
        objc_msgSend_CGPoint(minimizeButton, selSetFrameOrigin, newMinimizeButtonPosition);        
        objc_msgSend_CGPoint(zoomButton, selSetFrameOrigin, newZoomButtonPosition);

    }

    private void HideZoomButton(IntPtr nsWindow) {
        var selStandardWindowButton = sel_registerName("standardWindowButton:");
        var selSetHidden = sel_registerName("setHidden:");

        var zoomButton = objc_msgSend_IntPtr_IntPtr(nsWindow, selStandardWindowButton, (IntPtr)3); // NSWindowButtonZoom
        
        objc_msgSend_Bool(zoomButton,selSetHidden, true);
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct CGPoint {
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

    [DllImport("libobjc.dylib", EntryPoint = "objc_msgSend")]
    private static extern IntPtr objc_msgSend_Bool(IntPtr receiver, IntPtr selector, bool arg1);
    
    [DllImport("libobjc.dylib", EntryPoint = "sel_registerName")]
    private static extern IntPtr sel_registerName(string name);
}

public static class Dlfcn { 
    [DllImport("libSystem.B.dylib", EntryPoint = "dlopen")] 
    public static extern IntPtr dlopen(string path, int mode); 

    [DllImport("libSystem.B.dylib", EntryPoint = "dlsym")] 
    public static extern IntPtr dlsym(IntPtr handle, string symbol); }