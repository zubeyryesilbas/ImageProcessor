using UnityEngine;

public class TiltShiftEffect : MonoBehaviour
{
    public Texture2D inputTexture;
    public int blurSize = 10;

    void Start()
    {
        if (inputTexture == null)
        {
            Debug.LogError("Input texture not assigned!");
            enabled = false;
        }

        ApplyTiltShiftEffect();
    }

    void ApplyTiltShiftEffect()
    {
        Color[] pixels = inputTexture.GetPixels();

        int width = inputTexture.width;
        int height = inputTexture.height;

        Color[] newPixels = new Color[width * height];

        // Apply horizontal blur
        for (int y = 0; y < height; y++)
        {
            for (int x = blurSize; x < width - blurSize; x++)
            {
                Color sum = new Color(0, 0, 0, 0);

                for (int i = -blurSize; i <= blurSize; i++)
                {
                    sum += pixels[y * width + x + i];
                }

                newPixels[y * width + x] = sum / (2 * blurSize + 1);
            }
        }

        // Apply vertical blur
        for (int x = 0; x < width; x++)
        {
            for (int y = blurSize; y < height - blurSize; y++)
            {
                Color sum = new Color(0, 0, 0, 0);

                for (int i = -blurSize; i <= blurSize; i++)
                {
                    sum += newPixels[(y + i) * width + x];
                }

                pixels[y * width + x] = sum / (2 * blurSize + 1);
            }
        }

        // Create a new texture with the modified pixels
        Texture2D outputTexture = new Texture2D(width, height);
        outputTexture.SetPixels(pixels);
        outputTexture.Apply();

        // Save the texture as PNG
        byte[] bytes = outputTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/Output.png", bytes);
    }
}