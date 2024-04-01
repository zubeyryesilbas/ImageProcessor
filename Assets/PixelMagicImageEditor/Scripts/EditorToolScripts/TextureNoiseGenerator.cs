namespace PixelMagicEditor
{
    using UnityEngine;
    using System.IO;

    public static class TextureNoiseGenerator
    {
        public static Texture2D GenerateNoise(Texture2D inputTexture, float amountOfNoise, float strengthOfNoise)
        {
            Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
            for (int y = 0; y < inputTexture.height; y++)
            {
                for (int x = 0; x < inputTexture.width; x++)
                {
                    Color originalColor = inputTexture.GetPixel(x, y);
                    Color noisyColor = AddNoise(originalColor, amountOfNoise, strengthOfNoise);

                    outputTexture.SetPixel(x, y, noisyColor);
                }
            }

            outputTexture.Apply();
            return outputTexture;
        }

        private static Color AddNoise(Color originalColor, float amount, float strength)
        {
            float r = Mathf.Clamp01(originalColor.r + Random.Range(-amount, amount) * strength);
            float g = Mathf.Clamp01(originalColor.g + Random.Range(-amount, amount) * strength);
            float b = Mathf.Clamp01(originalColor.b + Random.Range(-amount, amount) * strength);
            float a = originalColor.a;

            return new Color(r, g, b, a);
        }
    }
}