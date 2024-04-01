namespace PixelMagicEditor
{
    using UnityEngine;

    public class VignetteEffect : MonoBehaviour
    {
        /* void ApplyVignetteEffect()
    {
        // Clone the input texture to avoid modifying the original
        Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);
        outputTexture.SetPixels(inputTexture.GetPixels());

        // Apply vignette effect to the cloned texture
        Color[] pixels = outputTexture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            // Calculate the distance from the center of the texture
            float distance = Vector2.Distance(new Vector2(i % outputTexture.width, i / outputTexture.width), 
                new Vector2(outputTexture.width / 2, outputTexture.height / 2));

            // Map the distance to a range between 0 and 1
            float vignetteIntensity = Mathf.Clamp01(1 - distance / (outputTexture.width / 2));

            // Apply vignette effect by adjusting the alpha channel
            pixels[i].a *= vignetteIntensity;
        }

        // Set the modified pixels back to the texture
        outputTexture.SetPixels(pixels);
        outputTexture.Apply();

        // Save the output texture as a PNG file
        byte[] bytes = outputTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/OutputTexture.png", bytes);

        Debug.Log("Vignette effect applied and saved as OutputTexture.png");
    }*/
    }
}