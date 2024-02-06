using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PixerEditor : EditorWindow
{
    private Texture2D inputImage;
    private Texture2D previewImage;
    private int imageWidth = 400;
    private Color[] _activeColorPalette;
    private ReorderableList colorList;
    private List<Color> colors;
    private int _intVal = 20;
    private float _floatVal;
    private bool _drawPixelArt;
    private float _timeExpanded;
    private float _edgeDetectionValue;
    private ColorPaletteHolderSO _colorPaletteHolder;
    private ProcessTypes _currentProcess = ProcessTypes.ConvertToPixelArt;
    private int selectedColorIndex;
    private List<Color> _colorsInPalette = new List<Color>();
    private bool _boolVal = false;
    private Color _colorVal;
    private Color _colorVal2;
    private float  _amountOfNoise = 0f;
    
    [MenuItem("Window/Pixer Editor")]
    public static void ShowWindow()
    {
        GetWindow<PixerEditor>("Pixer");
    }

    private void OnGUI()
    {   
        
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Pixer Editor", EditorStyles.boldLabel );
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        DrawButtonsSection();
        DrawInputImageSection();
        DrawOutputPreviewSection();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        switch (_currentProcess)
        {
            case ProcessTypes.ConvertToPixelArt :
                DrawColorPaletteOption();
                DrawPixelArt();
                break;
            case ProcessTypes.ConvertToGrayScale :
                DrawGrayScale();
                break;
            case ProcessTypes.AdjustBrightness:
                AdjustBrightness();
                break;
            case ProcessTypes.DarkenTexture:
                DarkenTexture();
                break;
            case ProcessTypes.RemapTexture:
                DrawColorPaletteOption();
                DrawRemap();
                break;
            case ProcessTypes.InvertColors:
                DrawInvert();
                break;
            case ProcessTypes.Sketch:
                Sketch();
                break;
            case ProcessTypes.SaturationAdjustment:
                DrawSaturation();
                break;
            case ProcessTypes.Posterize:
                DrawPosterize();
                break;
            case ProcessTypes.AdjustContrast:
                DrawConstrast();
                break;
            case ProcessTypes.Threshold:
                DrawTreshHold();
                break;
            case ProcessTypes.SharpenTexture:
                SharpenTexture();
                break;
            case ProcessTypes.SolarizeEffect:
                DrawSolarize();
                break;
            case ProcessTypes.BlurTexture :
                DrawBlur();
                break;
            case ProcessTypes.DuotoneEffect:
                DrawDuoToneEffect();
                break;
            case ProcessTypes.EmbossEffect:
                DrawEmbossEffect();
                break;
            case ProcessTypes.GlowEffect:
                DrawGlowEffect();
                break;
            case ProcessTypes.HueDisplacement:
                DrawHueDisplacement();
                break;
            case ProcessTypes.NoiseGenerator:
                DrawNoise();
                break;
        }
    }

    private void DrawNoise()
    {
        EditorGUILayout.BeginHorizontal();
        DrawFloat(0.01f , 10f ,"Amount Of Noise");
        var header = "Strength of Noise";
      
        var min = 0.01f;
        var max = 10f;
     
        _amountOfNoise = EditorGUILayout.FloatField(header, _amountOfNoise);
        if (_amountOfNoise <= min)
            _amountOfNoise = min;

        if (_amountOfNoise> max)
            _amountOfNoise = max;
        
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Apply Noise"))
        {
            previewImage = TextureNoiseGenerator.GenerateNoise(inputImage, _floatVal, _amountOfNoise);
        }
    }
    private void DrawHueDisplacement()
    {
        DrawFloat(0.01f , 10f , "hue shift amount");
        if (GUILayout.Button("Apply"))
        {
            previewImage = HueDisplacement.ApplyHueDisplacement(inputImage, _floatVal);
        }
    }
    private void DrawGlowEffect()
    {
        DrawFloat(0.01f , 10f , "Glow Intensity");
        if (GUILayout.Button("Add Glow Effect"))
        {
           previewImage =  AddGlowEffect.ApplyGlowEffect(inputImage, _floatVal);
        }
    }
    private void DrawEmbossEffect()
    {   
        GUILayout.BeginHorizontal();
        _boolVal = EditorGUILayout.Toggle("Gray Scale", _boolVal);
        DrawFloat(0.01f , 10f , "Emoboss Strength");
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Emboss"))
        {
            previewImage = EmbossEffect.ApplyEmbossEffect(inputImage, _floatVal, _boolVal);
        }
    }
    private void DrawDuoToneEffect()
    {   
        GUILayout.BeginHorizontal();
        GUILayout.Label("Shadow Color");
        _colorVal = EditorGUILayout.ColorField(_colorVal);
        _colorVal.a = 255f;
        GUILayout.Label("Highlight Color");
        _colorVal2 = EditorGUILayout.ColorField(_colorVal2);
        _colorVal2.a = 255f;
        GUILayout.EndHorizontal();
        if (GUILayout.Button("DuoTone"))
        {
            previewImage = DuotoneEffect.ApplyDuotoneEffect(inputImage, _colorVal, _colorVal2);
        }
        
    }
    private void DrawBlur()
    {
        DrawInt(15 , 0 , "Blur radius");
        if (GUILayout.Button("Blur Texture"))
        {
            previewImage = BlurImageProcessor.ApplyBlur(inputImage, _intVal);
        }
    }
    private void DrawSolarize()
    {
        DrawFloat(0.01f , 1f , "Solarize Amount");
        if (GUILayout.Button("Solarize "))
        {
            previewImage = SolarizeEffect.Solarize(inputImage, _floatVal);
        }
    }
    private void SharpenTexture()
    {
        DrawFloat(0.01f , 4f , "Sharpen Amount");
        if (GUILayout.Button("Sharpen Texture"))
        {
            previewImage = ImageSharpening.SharpenTexture(inputImage, _floatVal);
        }
    }
    private void DrawTreshHold()
    {
        DrawFloat(0.01f , 1f , "Threshold");
        if (GUILayout.Button("Threshold Texture"))
        {
            previewImage = ThresholdTexture.ApplyThreshold(inputImage, _floatVal);
        }
    }
    private void DrawConstrast()
    {
        DrawFloat(0.01f , 10f , "Contrast Amount");
        if (GUILayout.Button("Change Contrast"))
        {
            previewImage = TextureContrastAdjuster.AdjustContrast(inputImage, _floatVal);
        }
    }
    private void DrawPosterize()
    {
        DrawInt(20 , 0 , "Posterize Amount");
        var val = 20 - _intVal + 1;
        if (GUILayout.Button("Posterize"))
        {
            previewImage = PosterizeEffect.ApplyPosterizeEffect(inputImage, val);
        }
    }
    private void DarkenTexture()
    {   
        DrawFloat(0.02f , 100f , "Darkness Amount");
        if (GUILayout.Button("Darken Texture"))
        {
            previewImage = DarkenAndSaveTexture.DarkenAndSaveTextur(inputImage, _floatVal);
        }
    }
    private void DrawGrayScale()
    {   
        if(GUILayout.Button("Convert to Gray Scale"))
            previewImage = GrayscaleConverter.ConvertToGrayscale(inputImage);
    }

    private void DrawFloat(float min  , float max , string header)
    {
        _floatVal = EditorGUILayout.FloatField(header, _floatVal);
        if (_floatVal <= min)
            _floatVal = min;

        if (_floatVal > max)
            _floatVal = max;
    }
    private void OnEnable()
    {
        _colorPaletteHolder = Resources.Load<ColorPaletteHolderSO>(nameof(ColorPaletteHolderSO));
        colors = new List<Color>
        {
            Color.red, Color.green, Color.blue,
            Color.yellow, Color.cyan, Color.magenta,
            Color.white, Color.gray, Color.black
        };
        
        colorList = new ReorderableList(colors, typeof(Color), true, true, true, true);

        colorList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Color List");
        colorList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            Color color = colors[index];

            EditorGUI.BeginChangeCheck();
            color = EditorGUI.ColorField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), color);
            if (EditorGUI.EndChangeCheck())
            {
                colors[index] = color;
            }
        };

        colorList.onAddCallback = list =>
        {
            colors.Add(Color.white);
        };

        colorList.onRemoveCallback = list =>
        {
            if (list.index >= 0 && list.index < colors.Count)
            {
                colors.RemoveAt(list.index);
            }
        };
    }

    private void DrawSaturation()
    {
        DrawInt(100 , -100 , "Saturation Level");
        if (GUILayout.Button("Adjust Saturation"))
        {
            previewImage = SaturationAdjustment.AdjustSaturation(inputImage, _intVal);
        }
    }
    private void DrawColorPaletteOption()
    {
        GUILayout.Label("Color Dropdown Example", EditorStyles.boldLabel);
        
        _boolVal = EditorGUILayout.Toggle("Use Your Palette", _boolVal);
        // Dropdown button for color selection
        if (!_boolVal)
        {
            selectedColorIndex = EditorGUILayout.Popup("Select a color:", selectedColorIndex, GetColorNames());
            var colorsInPalette =  _colorPaletteHolder.ColorPalettes[selectedColorIndex  ].ColorsInPalette;
            _colorsInPalette.Clear();
            foreach (var item in colorsInPalette)
            {
               _colorsInPalette.Add(item);
            }

            foreach (var item in  _colorsInPalette)
            {
                EditorGUILayout.ColorField(item);
            }
        }
        else
        {
            colorList.DoLayoutList();
        }
    }

    private  string [] GetColorNames()
    {
        var length = _colorPaletteHolder.ColorPalettes.Count +1;
        var stringArray = new string[length];
        for (var i = 0  ; i < length-1 ; i ++)
        {
            stringArray[i] = _colorPaletteHolder.ColorPalettes[i].Id;
        }

        return stringArray;
    }
    private void DrawRemap()
    {   
        if (GUILayout.Button("Remap"))
        {
            if (_boolVal)
            {
                previewImage = TextureColorMapper.MapColorsToPalette(inputImage, colors.ToArray());
            }
            else
            {
                previewImage = TextureColorMapper.MapColorsToPalette(inputImage, _colorsInPalette.ToArray());
            }
            
        }
          
    }
    private void DrawPixelArt()
    {
        DrawInt(1 , 50 , "Pixel Size");
        if(GUILayout.Button("Pixelate"))
        {
            if (_boolVal)
            {
                previewImage =  ImageToPixelArtConverter.ConvertToPixelArt(inputImage,  _intVal, colors.ToArray());
            }
            else
            {
                previewImage =  ImageToPixelArtConverter.ConvertToPixelArt(inputImage,  _intVal, _colorsInPalette.ToArray());
            }
            
        }
    }

    private void DrawInvert()
    {
        if (GUILayout.Button("Invert"))
        {
            previewImage = ImageColorInverter.InvertColors(inputImage);
        }
    }
    private void DrawInt(int maxVal , int minVal , string header)
    {
        _intVal = EditorGUILayout.IntField(header, _intVal);
        if (_intVal <= minVal)
            _intVal = minVal;
       
        else if( _intVal >= maxVal)
        {
            _intVal = maxVal;
        }
    }
    private void AdjustBrightness()
    {   
        DrawFloat(0.02f , 20f , "Brightness Amount");
        if(GUILayout.Button("Execute"))
            previewImage = ImageBrightnessController.AdjustBrightness(inputImage, _floatVal);
    }
    private void DrawButtonsSection()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);

        GUILayout.Label("Buttons Section", EditorStyles.boldLabel);

        if (GUILayout.Button("Convert to PixelArt"))
        {
            _currentProcess = ProcessTypes.ConvertToPixelArt;
        }

        if (GUILayout.Button("Remap  Texture"))
        {
            _currentProcess = ProcessTypes.RemapTexture;
        }
        if (GUILayout.Button("Convert to Gray Scale"))
        {
            _currentProcess = ProcessTypes.ConvertToGrayScale;
        }

        if (GUILayout.Button("AdjustBrightness"))
        {
            _currentProcess = ProcessTypes.AdjustBrightness;
        }
        if (GUILayout.Button("Darken Texture"))
        {
            _currentProcess = ProcessTypes.DarkenTexture;
        }
        if (GUILayout.Button("Invert Colors"))
        {
            _currentProcess = ProcessTypes.InvertColors;
        }
        if (GUILayout.Button("Sketch"))
        {
            _currentProcess = ProcessTypes.Sketch;
        }

        if (GUILayout.Button("Adjust Saturation"))
        {
            _currentProcess = ProcessTypes.SaturationAdjustment;
        }

        if (GUILayout.Button("Posterize"))
        {
            _currentProcess = ProcessTypes.Posterize;
        }

        if (GUILayout.Button("Change Contrast"))
        {
            _currentProcess = ProcessTypes.AdjustContrast;
        }

        if (GUILayout.Button("ThreshHold Texture"))
        {
            _currentProcess = ProcessTypes.Threshold;
        }

        if (GUILayout.Button("Sharpen Texturer"))
        {
            _currentProcess = ProcessTypes.SharpenTexture;
        }

        if (GUILayout.Button("Solarize Effect"))
        {
            _currentProcess = ProcessTypes.SolarizeEffect;
        }

        if (GUILayout.Button("Apply Blur"))
        {
            _currentProcess = ProcessTypes.BlurTexture;
        }

        if (GUILayout.Button("Duotone Effect"))
        {
            _currentProcess = ProcessTypes.DuotoneEffect;
        }

        if (GUILayout.Button("Emboss Effect"))
        {
            _currentProcess = ProcessTypes.EmbossEffect;
        }

        if (GUILayout.Button("Glow Effect"))
        {
            _currentProcess = ProcessTypes.GlowEffect;
        }

        if (GUILayout.Button("Hue Displacement"))
        {
            _currentProcess = ProcessTypes.HueDisplacement;
        }

        if (GUILayout.Button("Add Noise"))
        {
            _currentProcess = ProcessTypes.NoiseGenerator;
        }
        GUILayout.EndVertical();
    }

    private void Sketch()
    {   
        GUILayout.BeginHorizontal();
        GUILayout.Label("Sketch Color");
        _colorVal = EditorGUILayout.ColorField(_colorVal );
        _colorVal.a = 255f;
        DrawFloat(0.001f , 0.5f , "Sketch Treshold");
        GUILayout.EndHorizontal();
        if(GUILayout.Button("Sketch"))
            previewImage = ImageSketcher.CreateSketch(inputImage, _floatVal, _colorVal);
    }
    private void DrawInputImageSection()
    {   
        GUILayout.BeginVertical(EditorStyles.helpBox);
        // Horizontal alignment for label and input field
        GUILayout.Label("Input Image", GUILayout.Width(80)); // Adjust the width as needed
        // Display Input Image
        inputImage = (Texture2D)EditorGUILayout.ObjectField(inputImage, typeof(Texture2D), false, GUILayout.Width(imageWidth), GUILayout.Height(imageWidth));
        //  GUILayout.Box(inputImage, GUILayout.Width(imageWidth), GUILayout.Height(imageWidth)); 
        GUILayout.EndVertical();
    }
    
    private void DrawOutputPreviewSection()
    {   
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Output Preview", GUILayout.Width(100));
        GUILayout.Box(previewImage , GUILayout.Width(imageWidth), GUILayout.Height(imageWidth));
        //var temp = (Texture2D)EditorGUILayout.ObjectField(inputImage, typeof(Texture2D), false, GUILayout.Width(imageWidth), GUILayout.Height(imageWidth));
        GUILayout.EndVertical();
    }
}
