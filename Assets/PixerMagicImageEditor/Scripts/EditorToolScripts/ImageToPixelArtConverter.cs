namespace PixerEditor
{
    using UnityEngine;
    using System.IO;

    public static class ImageToPixelArtConverter
    {
        public static Texture2D ConvertToPixelArt(Texture2D texture, int pixelSize, Color[] colorPalette)
        {
            int width = texture.width;
            int height = texture.height;
            // Create a new texture for pixel art
            Texture2D pixelArtTexture = new Texture2D(width, height);
            pixelArtTexture.filterMode = FilterMode.Point;

            for (int x = 0; x < width; x += pixelSize)
            {
                for (int y = 0; y < height; y += pixelSize)
                {
                    // Average color values within the pixel block
                    Color averageColor = GetAverageColor(texture, x, y, pixelSize);

                    // Find the closest color in the palette
                    Color closestColor = FindClosestColor(averageColor, colorPalette);

                    // Fill the pixel block with the closest color
                    for (int i = 0; i < pixelSize; i++)
                    {
                        for (int j = 0; j < pixelSize; j++)
                        {
                            var color = Color.clear;
                            if (texture.GetPixel(x + i, y + j) != Color.clear)
                                color = closestColor;
                            pixelArtTexture.SetPixel(x + i, y + j, color);
                        }
                    }
                }
            }

            // Apply changes to the new texture
            pixelArtTexture.Apply();

            return pixelArtTexture;
        }

        private static Color GetAverageColor(Texture2D texture, int startX, int startY, int size)
        {
            Color averageColor = Color.black;

            for (int x = startX; x < startX + size; x++)
            {
                for (int y = startY; y < startY + size; y++)
                {
                    averageColor += texture.GetPixel(x, y);
                }
            }

            // Calculate the average color
            averageColor /= (size * size);

            return averageColor;
        }

        private static Color FindClosestColor(Color targetColor, Color[] palette)
        {
            Color closestColor = palette[0];
            float minDistance = Mathf.Infinity;
            if (targetColor == Color.clear)
                return Color.clear;
            foreach (Color paletteColor in palette)
            {
                float distance = ColorDistance(targetColor, paletteColor);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestColor = paletteColor;
                }
            }

            return closestColor;
        }

        private static float ColorDistance(Color color1, Color color2)
        {
            float r = color1.r - color2.r;
            float g = color1.g - color2.g;
            float b = color1.b - color2.b;

            return Mathf.Sqrt(r * r + g * g + b * b);
        }

        private static void SaveTextureToFile(Texture2D texture, string filePath)
        {
            // Convert the texture to bytes
            byte[] bytes = texture.EncodeToPNG();

            // Write the bytes to the file
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Pixel art saved to: " + filePath);
        }
    }

}