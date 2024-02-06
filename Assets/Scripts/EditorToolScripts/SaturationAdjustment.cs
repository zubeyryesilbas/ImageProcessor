using UnityEngine;

public static class SaturationAdjustment 
{
    public static Texture2D AdjustSaturation(Texture2D texture, int saturation)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];

            // Skip pure white and pure black colors
            if ((pixel.r == 1 && pixel.g == 1 && pixel.b == 1) || (pixel.r == 0 && pixel.g == 0 && pixel.b == 0))
                continue;

            Color convertedPixel = ColorConversion.RGBToHSV(pixel);

            // Adjust saturation more conservatively for pixels with low saturation
            if (convertedPixel.g < 0.5)
            {
                convertedPixel.g += saturation / 200f; // Adjust saturation more conservatively
            }
            else
            {
                convertedPixel.g += saturation / 100f;
            }

            convertedPixel.g = Mathf.Clamp(convertedPixel.g, 0f, 1f);

            // Apply a desaturation filter for grayscale colors to limit color shifts
            float desaturationFactor = 1.0f - convertedPixel.g;
            convertedPixel.r += (pixel.r - convertedPixel.r) * desaturationFactor;
            convertedPixel.b += (pixel.b - convertedPixel.b) * desaturationFactor;

            pixels[i] = ColorConversion.HSVToRGB(convertedPixel);
        }

        var newTexture = new Texture2D(texture.width , texture.height);
        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
    }
}
public static class ColorConversion
{
    public static Color RGBToHSV(Color rgb)
    {
        float max = Mathf.Max(rgb.r, Mathf.Max(rgb.g, rgb.b));
        float min = Mathf.Min(rgb.r, Mathf.Min(rgb.g, rgb.b));
        float chroma = max - min;

        float hue = 0f;
        if (chroma != 0f)
        {
            if (max == rgb.r)
                hue = (rgb.g - rgb.b) / chroma;
            else if (max == rgb.g)
                hue = 2f + (rgb.b - rgb.r) / chroma;
            else
                hue = 4f + (rgb.r - rgb.g) / chroma;

            hue = Mathf.Repeat(hue * 60f, 360f);
        }

        float value = max;
        float saturation = (max != 0f) ? chroma / max : 0f;

        return new Color(hue / 360f, saturation, value, rgb.a);
    }

    public static Color HSVToRGB(Color hsv)
    {
        float chroma = hsv.b * hsv.g;
        float huePrime = hsv.r * 6f;
        float secondary = chroma * (1f - Mathf.Abs(Mathf.Repeat(huePrime, 2f) - 1f));

        float r, g, b;

        if (huePrime >= 0f && huePrime < 1f)
        {
            r = chroma;
            g = secondary;
            b = 0f;
        }
        else if (huePrime >= 1f && huePrime < 2f)
        {
            r = secondary;
            g = chroma;
            b = 0f;
        }
        else if (huePrime >= 2f && huePrime < 3f)
        {
            r = 0f;
            g = chroma;
            b = secondary;
        }
        else if (huePrime >= 3f && huePrime < 4f)
        {
            r = 0f;
            g = secondary;
            b = chroma;
        }
        else if (huePrime >= 4f && huePrime < 5f)
        {
            r = secondary;
            g = 0f;
            b = chroma;
        }
        else
        {
            r = chroma;
            g = 0f;
            b = secondary;
        }

        float min = hsv.b - chroma;
        r += min;
        g += min;
        b += min;

        return new Color(r, g, b, hsv.a);
    }
}
