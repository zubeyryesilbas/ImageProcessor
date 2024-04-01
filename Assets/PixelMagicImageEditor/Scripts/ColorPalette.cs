using System;

namespace  PixelMagicEditor
{
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class ColorPalette
    {
        public ColorPalette(List<Color> colors, string id)
        {
            ColorsInPalette = colors;
            Id = id;
        }

        public List<Color> ColorsInPalette = new List<Color>();
        public string Id;
    }
}