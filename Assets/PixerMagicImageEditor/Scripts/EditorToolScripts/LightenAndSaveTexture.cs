namespace PixerEditor
{
    using UnityEngine;
    using System.IO;

    public class LightenAndSaveTexture : MonoBehaviour
    {
        public Texture2D darkenedTexture; // Assign your darkened texture in the inspector
        public float brightnessAmount = 50f; // Adjust this value between 1 and 100
        public string outputFilePath = "LightenedTexture.png"; // Change the file name or extension as needed

        void Start()
        {
            LightenAndSaveTextur(darkenedTexture, brightnessAmount, outputFilePath);
        }

        void LightenAndSaveTextur(Texture2D darkenedTexture, float amount, string filePath)
        {
            // Create a new Texture2D with supported format
            Texture2D lightenedTexture = new Texture2D(darkenedTexture.width, darkenedTexture.height,
                TextureFormat.RGBA32, darkenedTexture.mipmapCount > 1);

            // Copy pixels from the darkened texture to the lightened texture
            lightenedTexture.SetPixels(darkenedTexture.GetPixels());
            lightenedTexture.Apply();

            // Lighten the texture to reverse the darkening effect
            LightenTexture(lightenedTexture, amount);

            // Save the lightened texture as PNG or JPG
            SaveTextureAsPNG(lightenedTexture, filePath);
            // Or use SaveTextureAsJPG(lightenedTexture, filePath);
        }

        void LightenTexture(Texture2D texture, float amount)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                // Lighten the color based on the amount
                pixels[i] /= (1f - amount / 100f);
            }

            texture.SetPixels(pixels);
            texture.Apply();
        }

        void SaveTextureAsPNG(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Texture saved as PNG: " + filePath);
        }

        void SaveTextureAsJPG(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Texture saved as JPG: " + filePath);
        }
    }

}