namespace PixelMagicEditor
{
    using UnityEngine;

    public static class OutlineDetection
    {
        public static Texture2D DetectOutline(Texture2D binaryTexture)
        {
            // Create a new texture to store the outline
            Texture2D outlineTexture = new Texture2D(binaryTexture.width, binaryTexture.height);

            // Iterate through each pixel of the binary texture
            for (int y = 0; y < binaryTexture.height; y++)
            {
                for (int x = 0; x < binaryTexture.width; x++)
                {
                    // If the pixel is part of the outline (e.g., it's an edge pixel), set it to white; otherwise, set it to transparent
                    if (IsOutlinePixel(binaryTexture, x, y))
                        outlineTexture.SetPixel(x, y, Color.white);
                    else
                        outlineTexture.SetPixel(x, y, Color.clear);
                }
            }

            // Apply changes and return the outline texture
            outlineTexture.Apply();
            return outlineTexture;
        }

        private static bool IsOutlinePixel(Texture2D binaryTexture, int x, int y)
        {
            // Check if the pixel is an edge pixel by examining its neighbors
            // You can implement a more sophisticated edge detection algorithm here
            // For simplicity, this example considers a pixel to be an edge pixel if any of its neighbors are different
            Color pixelColor = binaryTexture.GetPixel(x, y);

            if (pixelColor == Color.black) // Only check if the pixel is part of the shape
            {
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    for (int offsetX = -1; offsetX <= 1; offsetX++)
                    {
                        // Skip the center pixel
                        if (offsetX == 0 && offsetY == 0)
                            continue;

                        int neighborX = x + offsetX;
                        int neighborY = y + offsetY;

                        // Check if the neighbor is within the texture bounds
                        if (neighborX >= 0 && neighborX < binaryTexture.width && neighborY >= 0 &&
                            neighborY < binaryTexture.height)
                        {
                            // If the neighbor is not part of the shape, the current pixel is an outline pixel
                            if (binaryTexture.GetPixel(neighborX, neighborY) == Color.clear)
                                return true;
                        }
                        else
                        {
                            // If the neighbor is outside the texture bounds, consider it as an outline pixel
                            return true;
                        }
                    }
                }
            }

            return false; // If none of the neighbors are different, it's not an outline pixel
        }
    }

}