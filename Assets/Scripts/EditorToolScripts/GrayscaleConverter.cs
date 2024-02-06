using UnityEngine;
using UnityEditor;

public static class GrayscaleConverter 
{
    
    public static Texture2D ConvertToGrayscale(Texture2D originalTexture)
    {
        // Create a new texture with the same dimensions
        Texture2D grayscaleTexture = new Texture2D(originalTexture.width, originalTexture.height);

        // Get the color array from the original texture
        Color[] originalColors = originalTexture.GetPixels();

        // Loop through each pixel and set the grayscale value
        for (int i = 0; i < originalColors.Length; i++)
        {
            Color originalColor = originalColors[i];

            float grayscaleValue = originalColor.r * 0.3f + originalColor.g * 0.59f + originalColor.b * 0.11f;

            Color grayscaleColor = new Color(grayscaleValue, grayscaleValue, grayscaleValue, originalColor.a);

            originalColors[i] = grayscaleColor;
        }

        // Set the pixels of the new texture
        grayscaleTexture.SetPixels(originalColors);

        // Apply changes
        grayscaleTexture.Apply();

        return grayscaleTexture;
    }

   private static void SaveTextureAsAsset(Texture2D texture, string path)
    {
        // Convert the texture to bytes
        byte[] bytes;

        // Check if the path ends with .jpg or .jpeg to determine the encoding method
        if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
        {
            bytes = texture.EncodeToJPG();
        }
        else
        {
            bytes = texture.EncodeToPNG();
        }

        // Save the bytes to a file
        System.IO.File.WriteAllBytes(path, bytes);

        // Refresh the Asset Database
        AssetDatabase.Refresh();
    }
}
