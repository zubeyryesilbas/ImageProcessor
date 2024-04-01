
namespace PixelMagicEditor
{
    using UnityEngine;

    public static class EmbossEffect
    {

        public static Texture2D ApplyEmbossEffect(Texture2D texture, float embossStrength, bool enableGrayscale)
        {
            int width = texture.width;
            int height = texture.height;
            Color[] pixels = texture.GetPixels();

            float[,] embossFilter = new float[,]
            {
                { -2, -1, 0 },
                { -1, 1, 1 },
                { 0, 1, 2 }
            };

            Color[] newPixels = new Color[width * height];

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    float embossValueR = 0, embossValueG = 0, embossValueB = 0;

                    for (int filterY = 0; filterY < 3; filterY++)
                    {
                        for (int filterX = 0; filterX < 3; filterX++)
                        {
                            int imageX = x - 1 + filterX;
                            int imageY = y - 1 + filterY;

                            Color pixel = pixels[imageY * width + imageX];

                            embossValueR += pixel.r * embossFilter[filterY, filterX];
                            embossValueG += pixel.g * embossFilter[filterY, filterX];
                            embossValueB += pixel.b * embossFilter[filterY, filterX];
                        }
                    }

                    embossValueR = Mathf.Clamp01(embossValueR) * embossStrength;
                    embossValueG = Mathf.Clamp01(embossValueG) * embossStrength;
                    embossValueB = Mathf.Clamp01(embossValueB) * embossStrength;

                    Color embossColor = new Color(embossValueR, embossValueG, embossValueB, 1.0f);

                    if (enableGrayscale)
                    {
                        float grayscale = (embossColor.r + embossColor.g + embossColor.b) / 3.0f;
                        embossColor = new Color(grayscale, grayscale, grayscale, 1.0f);
                    }

                    newPixels[y * width + x] = embossColor;
                }
            }

            Texture2D resultTexture = new Texture2D(width, height);
            resultTexture.SetPixels(newPixels);
            resultTexture.Apply();
            return resultTexture;
        }
    }
}



