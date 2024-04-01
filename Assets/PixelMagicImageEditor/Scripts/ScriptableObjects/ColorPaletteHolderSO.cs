namespace PixelMagicEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ColorPalette", menuName = "New Color Palette", order = 1)]

    public class ColorPaletteHolderSO : ScriptableObject
    {
        public List<ColorPalette> ColorPalettes = new List<ColorPalette>();
    }

}