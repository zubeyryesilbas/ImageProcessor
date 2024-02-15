using UnityEngine;

namespace PixerEditor
{   
    public static class AlphaGradientGenerator 
    {
        public static Texture2D AdjustAlpha( Texture2D inputTexture ,GradientGenerator.GradientType gradientType , Gradient gradientColors , float rotationAngle)
        {
            var textureHeight = inputTexture.height;
            var textureWidth = inputTexture.width;
            Texture2D alphaTexture = new Texture2D(textureWidth, textureHeight);
            
            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    float normalizedX = x / (float)(textureWidth - 1);
                    float normalizedY = y / (float)(textureHeight - 1);

                    Color pixelColor = inputTexture.GetPixel(x , y);
                    if (gradientType == GradientGenerator.GradientType.Linear)
                    {
                        pixelColor.a = GetLinearGradientColor(normalizedX, normalizedY , gradientColors,rotationAngle).a;
                    }
                    else
                    {
                        pixelColor.a = GetRadialGradientColor(normalizedX, normalizedY , gradientColors).a;
                    }
    
                    alphaTexture.SetPixel(x, y, pixelColor);
                }
            }
    
            alphaTexture.Apply();
    
            return alphaTexture;
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