using UnityEngine;

public static class DuotoneEffect 
{
    public static Texture2D ApplyDuotoneEffect(Texture2D originalTexture ,Color shadowColor , Color highlightColor)
    {
        // Clone the original texture to avoid modifying the original asset
        Texture2D duotoneTexture = new Texture2D(originalTexture.width, originalTexture.height);

        // Get the pixels of the cloned texture
        Color[] pixels = originalTexture.GetPixels();

        // Apply duotone effect to each pixel
        for (int i = 0; i < pixels.Length; i++)
        {
            float grayscaleValue = pixels[i].grayscale;
            pixels[i] = Color.Lerp(shadowColor, highlightColor, grayscaleValue);
        }

        // Set the modified pixels to the new texture
        duotoneTexture.SetPixels(pixels);

        // Apply changes
        duotoneTexture.Apply();
        return duotoneTexture;
    }
}