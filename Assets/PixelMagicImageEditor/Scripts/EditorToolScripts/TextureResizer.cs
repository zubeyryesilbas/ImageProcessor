namespace PixelMagicEditor
{
    using UnityEngine;

    public static class TextureResizer
    {
        public static Texture2D ResizeTexture(Texture2D inputTexture, int outputWidth, int outputHeight,
            bool preserveAspectRatio)
        {
            // Calculate output width and height while preserving aspect ratio if required
            if (preserveAspectRatio)
            {
                float aspectRatio = (float)inputTexture.width / inputTexture.height;
                if (outputWidth == 0)
                {
                    outputWidth = Mathf.RoundToInt(outputHeight * aspectRatio);
                }
                else if (outputHeight == 0)
                {
                    outputHeight = Mathf.RoundToInt(outputWidth / aspectRatio);
                }
            }

            // Create a new Texture2D with the desired dimensions
            Texture2D outputTexture = new Texture2D(outputWidth, outputHeight);

            // Iterate through each pixel in the output texture
            for (int y = 0; y < outputHeight; y++)
            {
                for (int x = 0; x < outputWidth; x++)
                {
                    // Map the coordinates from the output texture space to the input texture space
                    float u = (float)x / outputWidth;
                    float v = (float)y / outputHeight;
                    int xCoord = Mathf.FloorToInt(u * inputTexture.width);
                    int yCoord = Mathf.FloorToInt(v * inputTexture.height);

                    // Get the color of the corresponding pixel in the input texture
                    Color color = inputTexture.GetPixel(xCoord, yCoord);

                    // Set the color of the current pixel in the output texture
                    outputTexture.SetPixel(x, y, color);
                }
            }

            // Apply changes to the output texture
            outputTexture.Apply();
            return outputTexture;
        }
    }

}