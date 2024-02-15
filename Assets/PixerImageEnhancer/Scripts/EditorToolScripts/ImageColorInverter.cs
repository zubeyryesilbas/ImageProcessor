namespace PixerEditor
{
    using UnityEngine;
    using System.IO;

    public static class ImageColorInverter
    {
        public static Texture2D InvertColors(Texture2D original)
        {
            Color[] pixels = original.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(1.0f - pixels[i].r, 1.0f - pixels[i].g, 1.0f - pixels[i].b, pixels[i].a);
            }

            Texture2D invertedTexture = new Texture2D(original.width, original.height);
            invertedTexture.SetPixels(pixels);
            invertedTexture.Apply();

            return invertedTexture;
        }

        private static void SaveTextureAsPNG(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Image saved as PNG: " + filePath);
        }

        private static void SaveTextureAsJPG(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Image saved as JPG: " + filePath);
        }
    }
}