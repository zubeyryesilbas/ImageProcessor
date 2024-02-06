using UnityEngine;

public class ImageBackgroundRemoval : MonoBehaviour
{
    public Texture2D originalImage; // Assign your image in the inspector
    public float colorTolerance = 0.1f; // Adjust this value based on your image

    void Start()
    {
        if (originalImage != null)
        {
            // Remove background
            Texture2D removedBackground = RemoveBackground(originalImage);

            // Save as PNG to Assets folder
            SaveAsPNG(removedBackground, "Assets/output_image");

            // Save as JPG to Assets folder
            SaveAsJPG(removedBackground, "Assets/output_image");
        }
        else
        {
            Debug.LogError("Original image is not assigned!");
        }
    }

    Texture2D RemoveBackground(Texture2D inputImage)
    {
        Color[] pixels = inputImage.GetPixels();
        Color backgroundColor = Color.white; // Set the background color to be removed

        for (int i = 0; i < pixels.Length; i++)
        {
            // Check if the color is similar to the background color within the tolerance
            if (ColorSimilarity(pixels[i], backgroundColor, colorTolerance))
            {
                pixels[i] = Color.clear; // Set the pixel to transparent
            }
        }

        // Create a new texture with the modified pixels
        Texture2D outputImage = new Texture2D(inputImage.width, inputImage.height);
        outputImage.SetPixels(pixels);
        outputImage.Apply();

        return outputImage;
    }

    bool ColorSimilarity(Color colorA, Color colorB, float tolerance)
    {
        float deltaR = Mathf.Abs(colorA.r - colorB.r);
        float deltaG = Mathf.Abs(colorA.g - colorB.g);
        float deltaB = Mathf.Abs(colorA.b - colorB.b);

        return deltaR <= tolerance && deltaG <= tolerance && deltaB <= tolerance;
    }

    void SaveAsPNG(Texture2D texture, string filePath)
    {
        byte[] pngBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath + ".png", pngBytes);
        Debug.Log("PNG saved as: " + filePath + ".png");
    }

    void SaveAsJPG(Texture2D texture, string filePath)
    {
        byte[] jpgBytes = texture.EncodeToJPG();
        System.IO.File.WriteAllBytes(filePath + ".jpg", jpgBytes);
        Debug.Log("JPG saved as: " + filePath + ".jpg");
    }
}
