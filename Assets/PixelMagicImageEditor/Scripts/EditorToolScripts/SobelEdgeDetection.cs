namespace PixelMagicEditor
{
    using UnityEngine;

    public static class SobelEdgeDetection
    {

        public static Texture2D DetectEdges(Texture2D texture, float edgeThreshold)
        {
            int width = texture.width;
            int height = texture.height;

            Texture2D edgesTexture = new Texture2D(width, height);
            Color[] pixels = texture.GetPixels();

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    // Apply the Sobel operator
                    float gx = CalculateSobelGradient(pixels, width, x, y, true);
                    float gy = CalculateSobelGradient(pixels, width, x, y, false);
                    float gradientMagnitude = Mathf.Sqrt(gx * gx + gy * gy);

                    // Apply edge threshold
                    Color edgeColor = gradientMagnitude > edgeThreshold ? Color.white : Color.black;

                    edgesTexture.SetPixel(x, y, edgeColor);
                }
            }

            edgesTexture.Apply();
            return edgesTexture;
        }

        static float CalculateSobelGradient(Color[] pixels, int width, int x, int y, bool horizontal)
        {
            int[,] kernel;

            if (horizontal)
            {
                kernel = new int[,]
                {
                    { -1, 0, 1 },
                    { -2, 0, 2 },
                    { -1, 0, 1 }
                };
            }
            else
            {
                kernel = new int[,]
                {
                    { -1, -2, -1 },
                    { 0, 0, 0 },
                    { 1, 2, 1 }
                };
            }

            float result = 0;

            for (int ky = -1; ky <= 1; ky++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    int pixelX = x + kx;
                    int pixelY = y + ky;

                    Color pixelColor = pixels[pixelY * width + pixelX];
                    float grayscale = (pixelColor.r + pixelColor.g + pixelColor.b) / 3.0f; // Convert to grayscale

                    result += kernel[ky + 1, kx + 1] * grayscale;
                }
            }

            return result;
        }
    }

}