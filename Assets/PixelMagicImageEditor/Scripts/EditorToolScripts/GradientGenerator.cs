

namespace PixelMagicEditor
{   
    using UnityEngine;
    public static class GradientGenerator 
    {
        public enum GradientType
        {
            Linear,
            Radial
        }
        
        public static Texture2D GenerateGradientTexture(int textureWidth , int textureHeight , GradientType gradientType , Gradient gradientColors , float rotationAngle)
        {
            Texture2D gradientTexture = new Texture2D(textureWidth, textureHeight);
    
            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    float normalizedX = x / (float)(textureWidth - 1);
                    float normalizedY = y / (float)(textureHeight - 1);
    
                    Color pixelColor;
    
                    if (gradientType == GradientType.Linear)
                    {
                        pixelColor = GetLinearGradientColor(normalizedX, normalizedY , gradientColors,rotationAngle);
                    }
                    else
                    {
                        pixelColor = GetRadialGradientColor(normalizedX, normalizedY , gradientColors);
                    }
    
                    gradientTexture.SetPixel(x, y, pixelColor);
                }
            }
    
            gradientTexture.Apply();
    
            return gradientTexture;
        }
    
        static Color GetLinearGradientColor(float x, float y , Gradient gradientColors , float rotationAngle )
        {
            float normalizedX = Mathf.Clamp01(Vector2.Dot(new Vector2(Mathf.Cos(rotationAngle), Mathf.Sin(rotationAngle)), new Vector2(x * 2 - 1, y * 2 - 1)) * 0.5f + 0.5f);
            float gradientValue = normalizedX;
            return gradientColors.Evaluate(gradientValue);
        }
    
    
        static  Color GetRadialGradientColor(float x, float y , Gradient gradientColors)
        {
            float normalizedDistance = Vector2.Distance(new Vector2(x, y), new Vector2(0.5f, 0.5f)) / 0.5f;
            float gradientValue = Mathf.Clamp01(normalizedDistance);
            return gradientColors.Evaluate(gradientValue);
        }
        
    }
}
