using UnityEngine;

public class GradientGenerator : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public GradientType gradientType = GradientType.Linear;
    public Gradient gradientColors;
    public float rotationAngle = 0f;

    public enum GradientType
    {
        Linear,
        Radial
    }

    void Start()
    {
        GenerateGradientTexture();
    }

    void GenerateGradientTexture()
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
                    pixelColor = GetLinearGradientColor(normalizedX, normalizedY);
                }
                else
                {
                    pixelColor = GetRadialGradientColor(normalizedX, normalizedY);
                }

                gradientTexture.SetPixel(x, y, pixelColor);
            }
        }

        gradientTexture.Apply();

        SaveTextureAsPNG(gradientTexture, "GradientTexture.png");
    }

    Color GetLinearGradientColor(float x, float y)
    {
        float normalizedX = Mathf.Clamp01(Vector2.Dot(new Vector2(Mathf.Cos(rotationAngle), Mathf.Sin(rotationAngle)), new Vector2(x * 2 - 1, y * 2 - 1)) * 0.5f + 0.5f);
        float gradientValue = normalizedX;
        return gradientColors.Evaluate(gradientValue);
    }


    Color GetRadialGradientColor(float x, float y)
    {
        float normalizedDistance = Vector2.Distance(new Vector2(x, y), new Vector2(0.5f, 0.5f)) / 0.5f;
        float gradientValue = Mathf.Clamp01(normalizedDistance);
        return gradientColors.Evaluate(gradientValue);
    }


    void SaveTextureAsPNG(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);
    }
}
