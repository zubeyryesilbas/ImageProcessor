using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;

public class FrameCapture
{
    private RenderTexture renderTexture;
    private Texture2D capturedTexture;
    private List<Texture2D> capturedTextures = new List<Texture2D>();
    private int frameWidth = 512;
    private int frameHeight = 512;
    private float captureInterval = 1f / 30f;

    public FrameCapture()
    {
        renderTexture = new RenderTexture(frameWidth, frameHeight, 24);
        capturedTexture = new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false);
    }

    public async void Capture(Camera captureCamera, float duration )
    {   
        capturedTextures.Clear();
        captureCamera.targetTexture = renderTexture;
        float currentTime = 0f;
        var color = captureCamera.backgroundColor;
        while (currentTime < duration)
        {
            CaptureFrame(color);
            await Task.Delay(Mathf.RoundToInt(1000f * captureInterval));
            currentTime += captureInterval;
        }

        Save();
    }

    private void CaptureFrame(Color color)
    {
        RenderTexture.active = renderTexture;
        capturedTexture.ReadPixels(new Rect(0, 0, frameWidth, frameHeight), 0, 0);
        capturedTexture.Apply();

        Color[] pixels = capturedTexture.GetPixels();
        
        

        capturedTexture.SetPixels(pixels);
        capturedTexture.Apply();
        pixels = capturedTexture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (i == 0)
            {
                Debug.Log(pixels[i]);
                Debug.Log( color);
            }
            if (pixels[i] == color)
            {
                pixels[i] = Color.clear;
                Debug.Log(pixels[i]);
            }
        }

        RenderTexture.active = null;

        // Add captured texture to the list
        capturedTextures.Add(new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false));
        capturedTextures[capturedTextures.Count - 1].SetPixels(pixels);
    }
    
    private void Save()
    {
        int index = 0;
        // Create a new Texture2D to hold the final sprite sheet
        var val = Mathf.RoundToInt(Mathf.Sqrt(capturedTextures.Count)) * frameHeight;
        foreach (var tex in capturedTextures)
        {
            var product  =tex.EncodeToPNG();
            File.WriteAllBytes("Assets/Resources/" + index + ".png", product);
            index++;
            
        }
        var finalTexture2D = new Texture2D(val, val, TextureFormat.RGB24, false);

        // Pack textures into the final sprite sheet
        var uvs = finalTexture2D.PackTextures(capturedTextures.ToArray(), 0, frameWidth * capturedTextures.Count);

        // Encode the final texture to PNG and save it as an asset
        var bytes = finalTexture2D.EncodeToPNG();
        AssetDatabase.Refresh();
        
    }
}
