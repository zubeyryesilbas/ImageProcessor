using System.IO;

namespace PixerEditor
{
    using UnityEngine;

    public static class ThresholdTexture
    {
        public static Texture2D ApplyThreshold(Texture2D originalTexture, float threshold)
        {
            int width = originalTexture.width;
            int height = originalTexture.height;

            // Create a new texture with the same dimensions and a supported format
            Texture2D thresholdedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            Color[] pixels = originalTexture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                float grayscaleValue = pixels[i].grayscale;

                if (grayscaleValue > threshold)
                {
                    pixels[i] = Color.white;
                }
                else
                {
                    pixels[i] = Color.black;
                }
            }

            thresholdedTexture.SetPixels(pixels);
            thresholdedTexture.Apply();

            return thresholdedTexture;
        }

        private static void SaveTextureAsPNG(Texture2D texture, string fileName)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/" + fileName + ".png", bytes);
            Debug.Log("Texture saved as PNG: " + fileName);
        }
    }
}