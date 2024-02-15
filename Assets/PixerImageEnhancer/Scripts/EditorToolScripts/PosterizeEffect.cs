namespace PixerEditor
{
    using UnityEngine;

    public static class PosterizeEffect
    {
        public static Texture2D ApplyPosterizeEffect(Texture2D inputTexture, int levels)
        {
            Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

            for (int y = 0; y < inputTexture.height; y++)
            {
                for (int x = 0; x < inputTexture.width; x++)
                {
                    Color pixelColor = inputTexture.GetPixel(x, y);
                    Color posterizedColor = new Color(
                        Mathf.Floor(pixelColor.r * levels) / (levels - 1),
                        Mathf.Floor(pixelColor.g * levels) / (levels - 1),
                        Mathf.Floor(pixelColor.b * levels) / (levels - 1),
                        pixelColor.a
                    );
                    outputTexture.SetPixel(x, y, posterizedColor);
                }
            }

            outputTexture.Apply();
            return outputTexture;
        }
    }
}