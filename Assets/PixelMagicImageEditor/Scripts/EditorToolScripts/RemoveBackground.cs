namespace PixelMagicEditor
{
    using UnityEngine;

    public static class RemoveBackground
    {

        public static Texture2D RemoveBackGroud(Texture2D texture2D)
        {
            var tex = SobelEdgeDetection.DetectEdges(texture2D, 0.02f);
            return RemoveBackgroundOutsideEdges(texture2D, tex);
        }

        public static Texture2D RemoveBackgroundOutsideEdges(Texture2D texture, Texture2D edgesTexture)
        {
            int width = texture.width;
            int height = texture.height;

            Texture2D outputTexture = new Texture2D(width, height);
            Color[] pixels = texture.GetPixels();
            Color[] edgesPixels = edgesTexture.GetPixels();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    // If pixel is outside of the detected edges, set alpha to 0
                    if (edgesPixels[index].r == 0)
                    {
                        pixels[index].a = 0;
                    }

                    outputTexture.SetPixel(x, y, pixels[index]);
                }
            }

            outputTexture.Apply();
            return outputTexture;
        }
    }

}