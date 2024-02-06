using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public static class ImageSketcher
{
   
    public static Texture2D CreateSketch( Texture2D inputImage ,  float edgeDetectionThreshold ,Color sketchColor )
    {
        // Convert the input image to grayscale
        Texture2D grayscaleImage = ConvertToGrayscale(inputImage);

        // Apply edge detection
        Texture2D edgeDetectedImage = ApplyEdgeDetection(grayscaleImage , edgeDetectionThreshold);

        // Apply sketch effect
        Texture2D sketchImage = ApplySketchEffect(edgeDetectedImage , edgeDetectionThreshold , sketchColor);

        // Display the sketch image
        return sketchImage;

      
    }

   private static Texture2D ConvertToGrayscale(Texture2D inputTexture)
    {
        Texture2D grayscaleTexture = new Texture2D(inputTexture.width, inputTexture.height);

        for (int x = 0; x < inputTexture.width; x++)
        {
            for (int y = 0; y < inputTexture.height; y++)
            {
                Color pixel = inputTexture.GetPixel(x, y);
                float grayscaleValue = (pixel.r + pixel.g + pixel.b) / 3.0f;
                grayscaleTexture.SetPixel(x, y, new Color(grayscaleValue, grayscaleValue, grayscaleValue));
            }
        }

        grayscaleTexture.Apply();

        return grayscaleTexture;
    }

    private static Texture2D ApplyEdgeDetection(Texture2D inputTexture , float edgeDetectionThreshold)
    {
        Texture2D edgeTexture = new Texture2D(inputTexture.width, inputTexture.height);

        for (int x = 1; x < inputTexture.width - 1; x++)
        {
            for (int y = 1; y < inputTexture.height - 1; y++)
            {
                Color centerPixel = inputTexture.GetPixel(x, y);
                Color[] neighbors = {
                    inputTexture.GetPixel(x - 1, y - 1), inputTexture.GetPixel(x, y - 1), inputTexture.GetPixel(x + 1, y - 1),
                    inputTexture.GetPixel(x - 1, y),     /* centerPixel */               inputTexture.GetPixel(x + 1, y),
                    inputTexture.GetPixel(x - 1, y + 1), inputTexture.GetPixel(x, y + 1), inputTexture.GetPixel(x + 1, y + 1)
                };

                float gradient = CalculateGradient(centerPixel, neighbors);
                Color edgeColor = (gradient > edgeDetectionThreshold) ? Color.black : Color.white;
                edgeTexture.SetPixel(x, y, edgeColor);
            }
        }

        edgeTexture.Apply();

        return edgeTexture;
    }

   private static float CalculateGradient(Color centerPixel, Color[] neighbors)
    {
        float centerValue = (centerPixel.r + centerPixel.g + centerPixel.b) / 3.0f;

        float gradient = 0.0f;
        foreach (Color neighbor in neighbors)
        {
            float neighborValue = (neighbor.r + neighbor.g + neighbor.b) / 3.0f;
            gradient += Mathf.Abs(centerValue - neighborValue);
        }

        return gradient / 8.0f;
    }

   private static Texture2D ApplySketchEffect(Texture2D inputTexture , float edgeDetectionThreshold , Color sketchColor)
    {
        Texture2D sketchTexture = new Texture2D(inputTexture.width, inputTexture.height);

        for (int x = 0; x < inputTexture.width; x++)
        {
            for (int y = 0; y < inputTexture.height; y++)
            {
                Color edgePixel = inputTexture.GetPixel(x, y);
                Color sketchPixel = (edgePixel.r > edgeDetectionThreshold) ? sketchColor : Color.white;
                sketchTexture.SetPixel(x, y, sketchPixel);
            }
        }

        sketchTexture.Apply();

        return sketchTexture;
    }
}
