using Avalonia.Media;
using Avalonia.Media.Imaging;
using MinecraftLaunch.Base.Models.Authentication;
using MinecraftLaunch.Skin.Class.Fetchers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Extensions;

namespace WonderLab.Utilities;

public static class SkinUtil {
    public static Dictionary<Guid, IImmutableBrush> SkinAvatarCaches { get; } = [];

    public static async Task<byte[]> GetSkinDataAsync(Account account, CancellationToken cancellationToken = default) {
        return await Task.Run(async () => {
            return account switch {
                OfflineAccount => "resm:WonderLab.Assets.gawrgura-13490790.png".ToBytes(),
                MicrosoftAccount => await new MicrosoftSkinFetcher(account.Uuid.ToString("N")).GetSkinAsync(),
                YggdrasilAccount yAccount => await new YggdrasilSkinFetcher(yAccount.YggdrasilServerUrl, yAccount.Uuid.ToString("N")).GetSkinAsync(),
                _ => throw new NotSupportedException()
            };
        }, cancellationToken);
    }

    public static Bitmap CroppedSkinAvatar(byte[] bytes, int aspectRatio = 56) {
        ProcessSkinAvatar(bytes.ToSKBitmap(), aspectRatio, out var hair, out var head);
        SKBitmap result = hair is null
            ? head
            : MergeAvatar(hair, head);
        
        return result.ToBitmap();
    }

    #region 图像操作

    private static void ProcessSkinAvatar(SKBitmap skBitmap, int aspectRatio, out SKBitmap hair, out SKBitmap head) {
        hair = null;
        if (skBitmap.Width < 32 || skBitmap.Height < 32)
            throw new Exception($"图片大小不足，长为 {skBitmap.Height}，宽为 {skBitmap.Width}");

        int scale = skBitmap.Width / 64;
        if (skBitmap.Width >= 64 && skBitmap.Height >= 32) {
            var pixel1 = skBitmap.GetPixel(1, 1);
            var pixel2 = skBitmap.GetPixel(skBitmap.Width - 1, skBitmap.Height - 1);
            var pixel3 = skBitmap.GetPixel(skBitmap.Width - 2, skBitmap.Height / 2 - 2);
            var referencePixel = skBitmap.GetPixel(scale * 41, scale * 9);

            if (pixel1.Alpha == 0 || pixel2.Alpha == 0 || pixel3.Alpha == 0 ||
                (pixel1 != referencePixel && pixel2 != referencePixel && pixel3 != referencePixel)) {
                hair = ResizeImage(ClipImage(skBitmap, scale * 40, scale * 8, scale * 8, scale * 8), aspectRatio, aspectRatio);
            }
        }

        head = ResizeImage(ClipImage(skBitmap, scale * 8, scale * 8, scale * 8, scale * 8), aspectRatio - 8, aspectRatio - 8);
    }

    private static SKBitmap MergeAvatar(SKBitmap hair, SKBitmap head) {
        if (hair is null || head is null)
            throw new ArgumentException("Both imgFore and imgBack must not be null.");

        if (hair.Width <= head.Width || hair.Height <= head.Height)
            throw new ArgumentException("imgFore must be larger than imgBack.");

        int offsetX = (hair.Width - head.Width) / 2;
        int offsetY = (hair.Height - head.Height) / 2;

        var mergedBitmap = new SKBitmap(hair.Width, hair.Height);
        using var canvas = new SKCanvas(mergedBitmap);

        canvas.Clear(SKColors.Transparent);
        canvas.DrawBitmap(head, new SKRect(offsetX, offsetY, offsetX + head.Width, offsetY + head.Height));
        canvas.DrawBitmap(hair, new SKRect(0, 0, hair.Width, hair.Height));

        return mergedBitmap;
    }

    private static SKBitmap ResizeImage(SKBitmap source, int width, int height) {
        var resizedBitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(resizedBitmap);

        canvas.DrawBitmap(source, new SKRect(0, 0, width, height));
        return resizedBitmap;
    }

    private static SKBitmap ClipImage(SKBitmap source, int x, int y, int width, int height) {
        var clippedBitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(clippedBitmap);

        var destRect = new SKRectI(0, 0, width, height);
        var srcRect = new SKRectI(x, y, x + width, y + height);

        canvas.DrawBitmap(source, srcRect, destRect);
        return clippedBitmap;
    }

    #endregion
}
