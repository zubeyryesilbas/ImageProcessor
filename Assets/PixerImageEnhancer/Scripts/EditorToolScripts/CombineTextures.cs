namespace PixerEditor
{
    using UnityEngine;

    public class CombineTextures : MonoBehaviour
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public Texture2D albedoTexture;
        public Texture2D specularTexture;
        public Texture2D metallicTexture;
        public int textureWidth = 512;
        public int textureHeight = 512;

        void Start()
        {
            // Create a new texture
            Texture2D newTexture = new Texture2D(textureWidth, textureHeight);

            // Get the mesh and its UVs
            Mesh mesh = skinnedMeshRenderer.sharedMesh;
            Vector2[] uvs = mesh.uv;

            // Iterate over vertices
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                // Sample color from textures
                Color albedoColor = SampleColorFromTexture(uvs[i], albedoTexture);
                Color specularColor = SampleColorFromTexture(uvs[i], specularTexture);
                Color metallicColor = SampleColorFromTexture(uvs[i], metallicTexture);

                // Write colors to new texture based on UV coordinates
                newTexture.SetPixel((int)(uvs[i].x * textureWidth), (int)(uvs[i].y * textureHeight), albedoColor);
                // Example: You can combine/specify channels for other maps (specular, metallic, etc.)
            }

            // Apply changes and assign the new texture
            newTexture.Apply();
            skinnedMeshRenderer.material.mainTexture = newTexture;
        }

        Color SampleColorFromTexture(Vector2 uv, Texture2D texture)
        {
            // Calculate texture coordinates
            float u = uv.x * (texture.width - 1);
            float v = uv.y * (texture.height - 1);

            // Sample color using bilinear filtering
            Color color = texture.GetPixelBilinear(u / texture.width, v / texture.height);
            return color;
        }

    }
}
