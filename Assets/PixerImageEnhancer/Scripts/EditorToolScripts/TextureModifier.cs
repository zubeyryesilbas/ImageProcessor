using System.IO;

namespace PixerEditor
{
    using UnityEngine;

    public class TextureModifier : MonoBehaviour
    {
        [SerializeField] private Texture2D inputTexture;

        void Start()
        {
            // Example: Load an existing texture

            // Check if the texture is not null
            if (inputTexture != null)
            {
                // Apply the desired gradient to the texture
                Texture2D outputTexture = ApplyLinearGradient(inputTexture, Color.black, Color.clear);

                // Save the modified texture as a PNG file
                SaveTextureAsPNG(outputTexture, "ModifiedTexture.png");
            }
            else
            {
                Debug.LogError("Input Texture not found!");
            }
        }

        Texture2D ApplyLinearGradient(Texture2D inputTexture, Color startColor, Color endColor)
        {
            int width = inputTexture.width;
            int height = inputTexture.height;

            Texture2D outputTexture = new Texture2D(width, height);

            for (int x = 0; x < width; x++)
            {
                float t = Mathf.InverseLerp(0, width - 1, x);
                Color color = Color.Lerp(startColor, endColor, t);

                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = inputTexture.GetPixel(x, y);
                    Color blendedColor = Color.Lerp(pixelColor, color, color.a);
                    outputTexture.SetPixel(x, y, blendedColor);
                }
            }

            outputTexture.Apply();
            return outputTexture;
        }

        void SaveTextureAsPNG(Texture2D texture, string fileName)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(fileName, bytes);
            Debug.Log("Texture saved as " + fileName);
        }
    }
}