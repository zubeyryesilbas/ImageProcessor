namespace PixelMagicEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class RemovePixels
    {
        public static Texture2D RemoveSimilarPixelsByColorPalette(Texture2D textureToModify, Color[] colorPalette,
            float similarityThreshold)
        {

            // Get the pixels from the texture
            Color[] pixels = textureToModify.GetPixels();

            // Iterate through each pixel
            for (int i = 0; i < pixels.Length; i++)
            {
                // Check similarity of the pixel color with colors in the palette
                foreach (Color paletteColor in colorPalette)
                {
                    if (ColorSimilarity(pixels[i], paletteColor) > similarityThreshold)
                    {
                        // Set the pixel color to transparent
                        pixels[i] = Color.clear; // You can use any other color here as well
                        break; // Exit the loop once a similar color is found
                    }
                }
            }

            // Apply the modified pixels back to the texture
            var output = new Texture2D(textureToModify.width, textureToModify.height);
            output.SetPixels(pixels);
            output.Apply();
            return output;
        }

        private static float ColorSimilarity(Color color1, Color color2)
        {
            float r = color1.r - color2.r;
            float g = color1.g - color2.g;
            float b = color1.b - color2.b;

            return Mathf.Sqrt(r * r + g * g + b * b);
        }
    }

}