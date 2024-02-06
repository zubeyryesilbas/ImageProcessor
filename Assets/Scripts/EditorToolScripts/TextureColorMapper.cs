using System.IO;
using UnityEngine;

public static class TextureColorMapper
{
   public static Texture2D MapColorsToPalette(Texture2D inputTexture, Color[] palette)
    {
        int width = inputTexture.width;
        int height = inputTexture.height;

        Texture2D outputTexture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = inputTexture.GetPixel(x, y);

                // Find the closest color in the palette
                Color mappedColor = FindClosestColor(pixelColor, palette);

                outputTexture.SetPixel(x, y, mappedColor);
            }
        }

        outputTexture.Apply();
        return outputTexture;
    }

    private static Color FindClosestColor(Color targetColor, Color[] palette)
    {
        Color closestColor = palette[0];
        float minDistance = Mathf.Infinity;

        foreach (Color paletteColor in palette)
        {
            float distance = ColorDistance(targetColor, paletteColor);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestColor = paletteColor;
            }
        }

        return closestColor;
    }

    private static float ColorDistance(Color color1, Color color2)
    {
        float r = color1.r - color2.r;
        float g = color1.g - color2.g;
        float b = color1.b - color2.b;

        return Mathf.Sqrt(r * r + g * g + b * b);
    }
    
}
