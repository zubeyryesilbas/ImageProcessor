namespace PixerEditor
{
    using UnityEngine;

    public static class OutlineEffect
    {
        public static Texture2D CreateOutlineTexture(Texture2D originalTexture, bool preserveOriginalImage,
            Color outlineColor, int outlineThickness, int smoothingIterations)
        {
            // Create a new texture for the outline
            Texture2D outlineTexture = new Texture2D(originalTexture.width, originalTexture.height);

            // Copy original texture if needed
            if (preserveOriginalImage)
            {
                Color[] originalPixels = originalTexture.GetPixels();
                outlineTexture.SetPixels(originalPixels);
            }

            // Loop through each pixel of the original texture
            for (int x = 0; x < originalTexture.width; x++)
            {
                for (int y = 0; y < originalTexture.height; y++)
                {
                    // Check if the current pixel is an edge pixel
                    if (IsEdgePixel(originalTexture, x, y, outlineThickness))
                    {
                        // Set the pixel color to the outline color
                        outlineTexture.SetPixel(x, y, outlineColor);
                    }
                    else
                    {
                        if (!preserveOriginalImage)
                            outlineTexture.SetPixel(x, y, Color.clear);
                    }
                }
            }

            // Apply changes and set the outline texture
            outlineTexture.Apply();

            // Smooth the outline
            SmoothOutline(outlineTexture, originalTexture, outlineThickness, smoothingIterations);

            return outlineTexture;
        }

        static bool IsEdgePixel(Texture2D texture, int x, int y, int outlineThickness)
        {
            Color pixelColor = texture.GetPixel(x, y);

            // Check if the current pixel is transparent
            if (pixelColor.a == 0)
                return false;

            // Check if any adjacent pixels are transparent
            for (int i = x - outlineThickness; i <= x + outlineThickness; i++)
            {
                for (int j = y - outlineThickness; j <= y + outlineThickness; j++)
                {
                    if (i >= 0 && i < texture.width && j >= 0 && j < texture.height)
                    {
                        if (texture.GetPixel(i, j).a == 0)
                            return true;
                    }
                    else
                    {
                        // If the adjacent pixel is outside the texture boundaries, consider it as an edge pixel
                        return true;
                    }
                }
            }

            return false;
        }

        static void SmoothOutline(Texture2D texture, Texture2D originalTexture, int outlineThickness,
            int smoothingIterations)
        {
            Color[] originalPixels = texture.GetPixels();

            for (int i = 0; i < smoothingIterations; i++)
            {
                Color[] smoothedPixels = new Color[originalPixels.Length];

                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        if (IsEdgePixel(originalTexture, x, y, outlineThickness))
                        {
                            Color avgColor = GetAverageColor(texture, x, y, outlineThickness);
                            smoothedPixels[y * texture.width + x] = avgColor;
                        }
                        else
                        {
                            smoothedPixels[y * texture.width + x] = originalPixels[y * texture.width + x];
                        }
                    }
                }

                smoothedPixels.CopyTo(originalPixels, 0);
            }

            texture.SetPixels(originalPixels);
            texture.Apply();
        }

        static Color GetAverageColor(Texture2D texture, int x, int y, int outlineThickness)
        {
            int count = 0;
            float r = 0, g = 0, b = 0, a = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < texture.width && j >= 0 && j < texture.height)
                    {
                        if (IsEdgePixel(texture, i, j, outlineThickness))
                        {
                            Color pixelColor = texture.GetPixel(i, j);
                            r += pixelColor.r;
                            g += pixelColor.g;
                            b += pixelColor.b;
                            a += pixelColor.a;
                            count++;
                        }
                    }
                }
            }

            return new Color(r / count, g / count, b / count, a / count);
        }
    }

}