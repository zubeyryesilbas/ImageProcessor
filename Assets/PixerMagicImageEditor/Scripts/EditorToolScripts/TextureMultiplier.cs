namespace  PixerEditor
{
    using UnityEngine;

    public static class TextureMultiplier
    {
        public static Texture2D MultiplyTextures(Texture2D texture1, Texture2D texture2)
        {
            if (texture1.width != texture2.width || texture1.height != texture2.height)
            {
                Debug.LogError("Textures must have the same dimensions for multiplication.");
                return null;
            }

            int width = texture1.width;
            int height = texture1.height;

            Texture2D resultTexture = new Texture2D(width, height);

            Color[] pixels1 = texture1.GetPixels();
            Color[] pixels2 = texture2.GetPixels();

            Color[] resultPixels = new Color[pixels1.Length];

            for (int i = 0; i < pixels1.Length; i++)
            {
                resultPixels[i] = pixels1[i] * pixels2[i];
            }

            resultTexture.SetPixels(resultPixels);
            resultTexture.Apply();

            return resultTexture;
        }
    }
}