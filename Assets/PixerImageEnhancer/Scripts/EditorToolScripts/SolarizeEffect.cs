namespace PixerEditor
{
    using System.Collections;
    using UnityEngine;

    public static class SolarizeEffect
    {

        public static Texture2D Solarize(Texture2D inputTex, float threshold)
        {
            Texture2D outputTex = new Texture2D(inputTex.width, inputTex.height);

            Color[] pixels = inputTex.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                Color pixel = pixels[i];

                // Apply solarization effect
                pixel.r = pixel.r < threshold ? 1 - pixel.r : pixel.r;
                pixel.g = pixel.g < threshold ? 1 - pixel.g : pixel.g;
                pixel.b = pixel.b < threshold ? 1 - pixel.b : pixel.b;

                pixels[i] = pixel;
            }

            outputTex.SetPixels(pixels);
            outputTex.Apply();

            return outputTex;
        }
    }
}