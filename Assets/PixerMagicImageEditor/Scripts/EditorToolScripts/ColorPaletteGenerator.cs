using System.Collections.Generic;
using UnityEngine;

namespace PixerEditor
{
    public static class ColorPaletteGenerator 
    {
        public static List<Color> GeneratePalette(Texture2D texture , float colorDetectionSensitivity)
        {
            Color[] colors = texture.GetPixels();

            var palette = GetUniqueColors(colors , colorDetectionSensitivity);
            return palette;
        }

        static List<Color> GetUniqueColors(Color[] colors , float colorDetectionSensitivity)
        {
            List<Color> uniqueColors = new List<Color>();

            for (int i = 0; i < colors.Length; i++)
            {
                bool isUnique = true;

                for (int j = 0; j < uniqueColors.Count; j++)
                {
                    if (ColorSimilarity(colors[i], uniqueColors[j]) <colorDetectionSensitivity)
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    uniqueColors.Add(colors[i]);
                }
            }

            return uniqueColors;
        }

        static float ColorSimilarity(Color color1, Color color2)
        {
            // Calculate the Euclidean distance between the two colors
            float rDiff = color1.r - color2.r;
            float gDiff = color1.g - color2.g;
            float bDiff = color1.b - color2.b;
            float distance = Mathf.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);

            // Normalize the distance
            float maxDistance = Mathf.Sqrt(3); // Max distance between two colors in RGB space
            return distance / maxDistance;
        }
    }
}
