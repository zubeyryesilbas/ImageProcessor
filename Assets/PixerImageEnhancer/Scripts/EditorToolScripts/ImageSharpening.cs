using System.IO;

namespace PixerEditor
{
    using UnityEngine;

    public static class ImageSharpening
    {
        public static Texture2D SharpenTexture(Texture2D input, float amount)
        {
            int width = input.width;
            int height = input.height;

            Texture2D output = new Texture2D(width, height);

            Color[] pixels = input.GetPixels();

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    Color center = pixels[y * width + x];
                    Color top = pixels[(y + 1) * width + x];
                    Color bottom = pixels[(y - 1) * width + x];
                    Color left = pixels[y * width + x - 1];
                    Color right = pixels[y * width + x + 1];

                    Color newColor = center * 5 - (top + bottom + left + right) * amount;
                    newColor = new Color(Mathf.Clamp01(newColor.r), Mathf.Clamp01(newColor.g),
                        Mathf.Clamp01(newColor.b), 1.0f);

                    output.SetPixel(x, y, newColor);
                }
            }

            output.Apply();
            return output;
        }

        private static void SaveTextureAsPNG(Texture2D texture, string fileName)
        {
            byte[] bytes = texture.EncodeToPNG();
            string path = Application.dataPath + "/" + fileName + ".png";
            File.WriteAllBytes(path, bytes);
            Debug.Log("Texture saved to: " + path);
        }
    }
}