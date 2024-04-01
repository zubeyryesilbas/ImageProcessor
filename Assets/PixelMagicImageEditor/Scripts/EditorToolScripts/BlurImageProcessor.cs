

namespace PixelMagicEditor
{
    using UnityEngine;
    public static class BlurImageProcessor 
    {
        public static Texture2D ApplyBlur(Texture2D inputTexture, int radius)
        {
            int width = inputTexture.width;
            int height = inputTexture.height;
    
            Texture2D blurredTexture = new Texture2D(width, height);
    
            // Apply horizontal blur
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color blurredColor = CalculateBlurredColor(inputTexture, x, y, radius, true);
                    blurredTexture.SetPixel(x, y, blurredColor);
                }
            }
    
            blurredTexture.Apply();
    
            // Apply vertical blur
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color blurredColor = CalculateBlurredColor(blurredTexture, x, y, radius, false);
                    blurredTexture.SetPixel(x, y, blurredColor);
                }
            }
    
            blurredTexture.Apply();
    
            return blurredTexture;
        }
    
        private static Color CalculateBlurredColor(Texture2D inputTexture, int x, int y, int radius, bool horizontal)
        {
            Color accumulatedColor = Color.black;
            int numSamples = 0;
    
            for (int i = -radius; i <= radius; i++)
            {
                int sampleX = horizontal ? Mathf.Clamp(x + i, 0, inputTexture.width - 1) : x;
                int sampleY = horizontal ? y : Mathf.Clamp(y + i, 0, inputTexture.height - 1);
    
                Color sampleColor = inputTexture.GetPixel(sampleX, sampleY);
                accumulatedColor += sampleColor;
                numSamples++;
            }
    
            return accumulatedColor / numSamples;
        }
        
    }
}
