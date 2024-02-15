using UnityEngine;

namespace PixerEditor
{
    public static class AddGlowEffect 
    {
        public static Texture2D  ApplyGlowEffect(Texture2D originalTexture , float glowIntensity)
        {
            int width = originalTexture.width;
            int height = originalTexture.height;

            Color[] originalPixels = originalTexture.GetPixels();
            Color[] newPixels = new Color[originalPixels.Length];

            for (int i = 0; i < originalPixels.Length; i++)
            {
                Color pixel = originalPixels[i];
                pixel *= glowIntensity; // Adjust intensity
                newPixels[i] = pixel;
            }

            Texture2D newTexture = new Texture2D(width, height);
            newTexture.SetPixels(newPixels);
            newTexture.Apply();

            // Assign the modified texture back to the originalTexture if needed
            originalTexture = newTexture;
            return originalTexture;
        }
    }
}
