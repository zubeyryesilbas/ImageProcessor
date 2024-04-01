namespace PixelMagicEditor
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class RegionGrowing
    {

        private static Texture2D DetectEdges(Texture2D texture)
        {
            // Implement edge detection algorithm (Sobel, Canny, etc.)
            // For simplicity, we'll use a placeholder (already provided in previous examples)
            return DetectEdgesWithSobel(texture);
        }

        public static Texture2D DetectEdgesWithSobel(Texture2D texture)
        {
            SobelEdgeDetection.DetectEdges(texture, 0.2f);
            // This function returns a texture with detected edges
            // For simplicity, we'll reuse the previous Sobel implementation
            return DetectEdges(texture);
        }

        public static Texture2D SelectRegion(Texture2D edgesTexture)
        {
            int width = edgesTexture.width;
            int height = edgesTexture.height;

            Texture2D outputTexture = new Texture2D(width, height);
            Color32[] edgesPixels = edgesTexture.GetPixels32();

            // Find a seed point on the edge
            Vector2Int seedPoint = FindSeedPoint(edgesPixels, width, height);

            // Perform flood fill from the seed point to select region
            bool[,] visited = new bool[width, height];
            FloodFill(edgesPixels, visited, seedPoint, width, height);

            // Apply the selected region to the output texture
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    outputTexture.SetPixel(x, y, visited[x, y] ? Color.white : Color.black);
                }
            }

            outputTexture.Apply();
            return outputTexture;
        }

        private static Vector2Int FindSeedPoint(Color32[] pixels, int width, int height)
        {
            // Find a seed point on the edge (for simplicity, just pick the first edge pixel found)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (pixels[index].r > 200) // Assuming white indicates an edge
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            // If no edge pixel found, return a default seed point
            return new Vector2Int(width / 2, height / 2);
        }

        private static void FloodFill(Color32[] pixels, bool[,] visited, Vector2Int seedPoint, int width, int height)
        {
            // Simple flood fill algorithm to select region
            // For simplicity, consider pixels with intensity > 200 (white) as part of the edge
            // You may need to adjust this threshold based on your specific edge detection results

            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(seedPoint);

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Pop();
                int x = current.x;
                int y = current.y;

                if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y])
                    continue;

                int index = y * width + x;
                if (pixels[index].r > 200) // Assuming white indicates an edge
                {
                    visited[x, y] = true;
                    stack.Push(new Vector2Int(x + 1, y));
                    stack.Push(new Vector2Int(x - 1, y));
                    stack.Push(new Vector2Int(x, y + 1));
                    stack.Push(new Vector2Int(x, y - 1));
                }
            }
        }
    }

}