using Avalonia.Media;

namespace WonderLab.Extensions;

public static class ColorExtension {
    public static void ToFloatArrayNonAlloc(this Color c, float[] array) {
        array[0] = c.R / 255f;
        array[1] = c.G / 255f;
        array[2] = c.B / 255f;
    }

    public static Color FromUInt32(this uint color) => Color.FromUInt32(color);
}