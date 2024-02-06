using UnityEngine;

public static class ImageBrightnessController 
{
   public static Texture2D AdjustBrightness(Texture2D originalTexture, float brightnessFactor)
    {
        Texture2D modifiedTexture = new Texture2D(originalTexture.width, originalTexture.height);

        for (int x = 0; x < originalTexture.width; x++)
        {
            for (int y = 0; y < originalTexture.height; y++)
            {
                Color originalColor = originalTexture.GetPixel(x, y);

                // Adjust brightness
                Color modifiedColor = originalColor * brightnessFactor;

                // Ensure color values are clamped between 0 and 1
                modifiedColor.r = Mathf.Clamp(modifiedColor.r, 0f, 1f);
                modifiedColor.g = Mathf.Clamp(modifiedColor.g, 0f, 1f);
                modifiedColor.b = Mathf.Clamp(modifiedColor.b, 0f, 1f);

                modifiedTexture.SetPixel(x, y, modifiedColor);
            }
        }

        modifiedTexture.Apply();

        return modifiedTexture;
    }

   private static void SaveTextureAsPNG(Texture2D texture, string path)
    {
        byte[] pngData = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, pngData);
    }
}