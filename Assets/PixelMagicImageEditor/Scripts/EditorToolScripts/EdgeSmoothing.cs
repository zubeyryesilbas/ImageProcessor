using UnityEngine;

namespace PixelMagicEditor
{   
    public static class EdgeSmoothing 
    {
        public static Texture2D SmoothEdges(Texture2D inputTexture, int edgeThreshold)
        {
            var outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
            
            // Iterate over each pixel in the input texture
            for (int x = 0; x < inputTexture.width; x++)
            {
                for (int y = 0; y < inputTexture.height; y++)
                {
                    // Get the color of the current pixel
                    Color currentColor = inputTexture.GetPixel(x, y);
                    
                    // Check if the pixel is close to the edge
                    if (IsEdgePixel(inputTexture, x, y, edgeThreshold))
                    {
                        // Smooth the color of the edge pixel
                        Color smoothColor = SmoothColor(inputTexture, x, y);
                        outputTexture.SetPixel(x, y, smoothColor);
                    }
                    else
                    {
                        // Keep the original color for non-edge pixels
                        outputTexture.SetPixel(x, y, currentColor);
                    }
                }
            }
            
            // Apply changes
            outputTexture.Apply();
            return outputTexture;
        }

        static bool IsEdgePixel(Texture2D inputTexture, int x, int y, int edgeThreshold)
        {
            // Check if the pixel's color difference from its neighbors is above the threshold
            Color currentColor = inputTexture.GetPixel(x, y);
            float colorDifference = 0f;

            // Compare the color difference between neighboring pixels
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Color neighborColor = inputTexture.GetPixel(x + i, y + j);
                    colorDifference += Vector3.Distance(new Vector3(currentColor.r, currentColor.g, currentColor.b), new Vector3(neighborColor.r, neighborColor.g, neighborColor.b));
                }
            }

            // Calculate average color difference
            colorDifference /= 8;

            // Check if the average color difference is above the threshold
            return colorDifference > edgeThreshold;
        }

        static Color SmoothColor(Texture2D inputTexture, int x, int y)
        {
            // Apply a simple averaging technique to smooth the color of the edge pixel
            
            // Initialize variables to accumulate color components
            float r = 0f, g = 0f, b = 0f;
            int count = 0;
            
            // Iterate over neighboring pixels and accumulate color components
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighborX = Mathf.Clamp(x + i, 0, inputTexture.width - 1);
                    int neighborY = Mathf.Clamp(y + j, 0, inputTexture.height - 1);
                    Color neighborColor = inputTexture.GetPixel(neighborX, neighborY);
                    
                    r += neighborColor.r;
                    g += neighborColor.g;
                    b += neighborColor.b;
                    count++;
                }
            }
            
            // Calculate the average color
            Color averageColor = new Color(r / count, g / count, b / count);
            return averageColor;
        }
    }
}
