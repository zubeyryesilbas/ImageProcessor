using UnityEngine;

public static class TextureContrastAdjuster 
{
   
   public static Texture2D  AdjustContrast( Texture2D inputTexture ,float contrast)
    {
        // Create a new texture to store the adjusted pixels
        Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

        // Get the pixels from the input texture
        Color[] pixels = inputTexture.GetPixels();

        // Adjust the contrast of each pixel
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = ContrastAdjustment(pixels[i], contrast);
        }

        // Set the adjusted pixels to the output texture
        outputTexture.SetPixels(pixels);

        // Apply changes and update the texture
        outputTexture.Apply();
        return outputTexture;
    }

   private static Color ContrastAdjustment(Color originalColor, float contrast)
    {
        // Adjust the contrast using the formula
        Color adjustedColor = new Color(
            (originalColor.r - 0.5f) * contrast + 0.5f,
            (originalColor.g - 0.5f) * contrast + 0.5f,
            (originalColor.b - 0.5f) * contrast + 0.5f,
            originalColor.a
        );

        return adjustedColor;
    }
   
}