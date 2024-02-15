using UnityEngine;

namespace PixerEditor
{
    public static class BorderTextureGenerator 
    {
        public static Texture2D GenerateBorderTexture(Texture2D inputTexture ,int borderThickness , Color borderColor ,Color shadowColor, float alphaThreshold , float shadowBlurStrength , float shadowTransparency )
        {
            // Create a new texture with increased width and height
            Texture2D outputTexture = new Texture2D(inputTexture.width + 2 * borderThickness, inputTexture.height + 2 * borderThickness);

            // Set the entire texture to the border color
            for (int y = 0; y < outputTexture.height; y++)
            {
                for (int x = 0; x < outputTexture.width; x++)
                {
                    outputTexture.SetPixel(x, y, borderColor);
                }
            }

            // Copy the input texture to the inner region with alpha threshold and shadow
            for (int y = borderThickness; y < outputTexture.height - borderThickness; y++)
            {
                for (int x = borderThickness; x < outputTexture.width - borderThickness; x++)
                {
                    Color originalColor = inputTexture.GetPixel(x - borderThickness, y - borderThickness);

                    // Apply alpha threshold
                    if (originalColor.a > alphaThreshold)
                    {
                        // Apply shadow effect
                        Color finalColor = originalColor;
                        if (originalColor.a < 1.0f)
                        {
                            Color shadow = shadowColor * (1 - originalColor.a) * shadowTransparency;
                            finalColor = Color.Lerp(originalColor, shadow, shadowBlurStrength);
                        }

                        outputTexture.SetPixel(x, y, finalColor);
                    }
                }
            }

            // Apply changes to the texture
            outputTexture.Apply();

            return outputTexture;
        }
    
    }

}
