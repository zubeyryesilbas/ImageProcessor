

namespace  PixelMagicEditor
{   
    using UnityEngine;
    public static class HueDisplacement 
    {
        private  static Texture2D DuplicateTexture(Texture2D source , float hueShiftAmount)
        {
            // Create a new texture with the same dimensions
            Texture2D duplicate = new Texture2D(source.width, source.height);

            // Copy the pixels from the source to the duplicate
            duplicate.SetPixels(source.GetPixels());
            duplicate.Apply();

            return duplicate;
        }

        public static Texture2D  ApplyHueDisplacement(Texture2D texture , float hueShiftAmount)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                // Convert the pixel color to HSV
                Color.RGBToHSV(pixels[i], out float h, out float s, out float v);

                // Shift the hue by the specified amount
                h = (h + hueShiftAmount) % 1.0f;

                // Convert back to RGB and update the pixel color
                pixels[i] = Color.HSVToRGB(h, s, v);
            }

            // Apply the modified pixels to the texture
            var tempTex = new Texture2D(texture.width, texture.height);
            tempTex.SetPixels(pixels);
            tempTex.Apply();
            return tempTex;
        }
    
    }

}
