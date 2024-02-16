namespace  PixerEditor
{
    using UnityEngine;

    public static class ImageProcessingUtils
    {
        public static Texture2D Outline(Texture2D originalTexture, float threshold, int dilationSize)
        {
            Texture2D duplicateTexture = new Texture2D(originalTexture.width, originalTexture.height);
            duplicateTexture.SetPixels(originalTexture.GetPixels());
            duplicateTexture.Apply();

            // Apply edge detection algorithm (Sobel)
            Texture2D edgeTexture = EdgeDetection.SobelEdgeDetection(duplicateTexture);

            // Apply thresholding
            Texture2D thresholdedTexture = ApplyThreshold(edgeTexture, threshold);

            // Dilate the edges
            Texture2D dilatedTexture = Dilate(thresholdedTexture, dilationSize);

            // Overlay the dilated edges onto the original texture
            Texture2D finalTexture = Blend(originalTexture, dilatedTexture);
            return finalTexture;
        }

        // Thresholding function
        public static Texture2D ApplyThreshold(Texture2D inputTexture, float threshold)
        {
            Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

            Color[] pixels = inputTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                float grayscaleValue = (pixels[i].r + pixels[i].g + pixels[i].b) / 3f;
                if (grayscaleValue >= threshold)
                    pixels[i] = Color.white;
                else
                    pixels[i] = Color.black;
            }

            outputTexture.SetPixels(pixels);
            outputTexture.Apply();

            return outputTexture;
        }

        // Dilation function
        public static Texture2D Dilate(Texture2D inputTexture, int dilationSize)
        {
            Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

            Color[] pixels = inputTexture.GetPixels();
            Color[] dilatedPixels = new Color[pixels.Length];

            for (int y = 0; y < inputTexture.height; y++)
            {
                for (int x = 0; x < inputTexture.width; x++)
                {
                    bool isEdgePixel = false;
                    for (int j = -dilationSize; j <= dilationSize; j++)
                    {
                        for (int i = -dilationSize; i <= dilationSize; i++)
                        {
                            int neighborX = Mathf.Clamp(x + i, 0, inputTexture.width - 1);
                            int neighborY = Mathf.Clamp(y + j, 0, inputTexture.height - 1);
                            if (inputTexture.GetPixel(neighborX, neighborY) == Color.white)
                            {
                                isEdgePixel = true;
                                break;
                            }
                        }

                        if (isEdgePixel)
                            break;
                    }

                    dilatedPixels[y * inputTexture.width + x] = isEdgePixel ? Color.white : Color.black;
                }
            }

            outputTexture.SetPixels(dilatedPixels);
            outputTexture.Apply();

            return outputTexture;
        }

        // Blend function for overlaying two textures
        public static Texture2D Blend(Texture2D texture1, Texture2D texture2)
        {
            Texture2D outputTexture = new Texture2D(texture1.width, texture1.height);

            Color[] pixels1 = texture1.GetPixels();
            Color[] pixels2 = texture2.GetPixels();
            Color[] blendedPixels = new Color[pixels1.Length];

            for (int i = 0; i < pixels1.Length; i++)
            {
                blendedPixels[i] = Color.Lerp(pixels1[i], pixels2[i], pixels2[i].a);
            }

            outputTexture.SetPixels(blendedPixels);
            outputTexture.Apply();

            return outputTexture;
        }
    }

    public static class EdgeDetection
    {
        // Sobel edge detection function
        public static Texture2D SobelEdgeDetection(Texture2D inputTexture)
        {
            Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

            Color[] pixels = inputTexture.GetPixels();
            Color[] edgePixels = new Color[pixels.Length];

            for (int y = 1; y < inputTexture.height - 1; y++)
            {
                for (int x = 1; x < inputTexture.width - 1; x++)
                {
                    float gx = (
                        pixels[(y - 1) * inputTexture.width + (x - 1)].r * -1f +
                        pixels[(y - 1) * inputTexture.width + (x + 1)].r +
                        pixels[(y) * inputTexture.width + (x - 1)].r * -2f +
                        pixels[(y) * inputTexture.width + (x + 1)].r * 2f +
                        pixels[(y + 1) * inputTexture.width + (x - 1)].r * -1f +
                        pixels[(y + 1) * inputTexture.width + (x + 1)].r
                    );

                    float gy = (
                        pixels[(y - 1) * inputTexture.width + (x - 1)].r * -1f +
                        pixels[(y - 1) * inputTexture.width + (x)].r * -2f +
                        pixels[(y - 1) * inputTexture.width + (x + 1)].r * -1f +
                        pixels[(y + 1) * inputTexture.width + (x - 1)].r +
                        pixels[(y + 1) * inputTexture.width + (x)].r * 2f +
                        pixels[(y + 1) * inputTexture.width + (x + 1)].r
                    );

                    float magnitude = Mathf.Sqrt(gx * gx + gy * gy);
                    edgePixels[y * inputTexture.width + x] = new Color(magnitude, magnitude, magnitude);
                }
            }

            outputTexture.SetPixels(edgePixels);
            outputTexture.Apply();

            return outputTexture;
        }
    }

}