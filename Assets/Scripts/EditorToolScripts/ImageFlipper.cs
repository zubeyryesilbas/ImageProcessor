using UnityEngine;

public class ImageFlipper : MonoBehaviour
{
    public Texture2D originalTexture;
    void Start()
    {
        // Load the original image from the Resources folder

        // Flip the image horizontally
        Texture2D flippedTexture = FlipTextureHorizontal(originalTexture);

        // Save the flipped image as a PNG file
        SaveTextureAsPNG(flippedTexture, "Assets/FlippedImage.png");
    }

    Texture2D FlipTextureHorizontal(Texture2D originalTexture)
    {
        Color[] originalPixels = originalTexture.GetPixels();
        Color[] flippedPixels = new Color[originalPixels.Length];

        int width = originalTexture.width;
        int height = originalTexture.height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int originalIndex = y * width + x;
                int flippedIndex = y * width + (width - 1 - x);
                flippedPixels[flippedIndex] = originalPixels[originalIndex];
            }
        }

        Texture2D flippedTexture = new Texture2D(width, height);
        flippedTexture.SetPixels(flippedPixels);
        flippedTexture.Apply();

        return flippedTexture;
    }

    void SaveTextureAsPNG(Texture2D texture, string filePath)
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);
        Debug.Log("Image saved as: " + filePath);
    }
}