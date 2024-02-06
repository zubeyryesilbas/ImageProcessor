using UnityEngine;
using System.IO;

public static class DarkenAndSaveTexture 
{
    public static Texture2D DarkenAndSaveTextur(Texture2D texture, float amount)
    {
        // Create a new Texture2D with supported format
        Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, texture.mipmapCount > 1);
        // Copy pixels from the original texture to the new texture
        newTexture.SetPixels(texture.GetPixels());
        newTexture.Apply();
        // Darken the new texture
       return DarkenTexture(newTexture, amount);
       
    }

   private static Texture2D DarkenTexture(Texture2D texture, float amount)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            // Darken the color based on the amount
            pixels[i] *= (1f - amount / 100f);
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}