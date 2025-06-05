using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;

namespace WonderLab.Extensions;

public static class AvaloniaExtsnsion {
    public static Bitmap ToBitmap(this string uri) {
        var memoryStream = new MemoryStream();
        using var stream = AssetLoader.Open(new Uri(uri));
        stream!.CopyTo(memoryStream);
        memoryStream.Position = 0;

        return new Bitmap(memoryStream);
    }
}