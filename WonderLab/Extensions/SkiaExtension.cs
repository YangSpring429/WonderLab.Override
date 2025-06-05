using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;
using System;
using System.IO;

namespace WonderLab.Extensions;

public static class SkiaExtension {
    public static Bitmap ToBitmap(this SKBitmap skBitmap) {
        using var image = SKImage.FromBitmap(skBitmap);
        using var data = image.Encode();
        using var stream = new MemoryStream(data.ToArray());
        return new Bitmap(stream);
    }

    public static SKBitmap ToSKBitmap(this byte[] bytes) {
        return SKBitmap.Decode(bytes);
    }

    public static byte[] ToBytes(this string uri) {
        using var memoryStream = new MemoryStream();
        using var stream = AssetLoader.Open(new Uri(uri));
        stream!.CopyTo(memoryStream);
        memoryStream.Position = 0;

        return memoryStream.ToArray();
    }
}