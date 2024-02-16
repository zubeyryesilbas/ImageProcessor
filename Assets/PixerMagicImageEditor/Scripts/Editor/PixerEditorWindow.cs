using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace PixerEditor
{
    public class PixerEditor : EditorWindow
    {
        private Texture2D inputImage;
        private Texture2D previewImage;
        private Texture2D inputImage2;
        private int imageWidth = 400;
        private Color[] _activeColorPalette;
        private ReorderableList colorList;
        private List<Color> colors;
        private int _intVal = 20;
        private int _intVal2 = 1;
        private float _floatVal;
        private bool _drawPixelArt;
        private float _timeExpanded;
        private float _edgeDetectionValue;
        private float _floatVal2, _floatVal3;
        private ColorPaletteHolderSO _colorPaletteHolder;
        private ProcessTypes _currentProcess = ProcessTypes.ConvertToPixelArt;
        private int selectedColorIndex;
        private List<Color> _colorsInPalette = new List<Color>();
        private bool _boolVal = false;
        private Color _colorVal;
        private Color _colorVal2;
        private float  _amountOfNoise = 0f;
        private Gradient _gradient = new Gradient();
        private string _colorPaletteName;
        private GradientGenerator.GradientType _gradientType;
        [SerializeField] private Object selectedObject;
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 _scrollPosition2 = Vector2.zero;
        
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
            if(_currentProcess == ProcessTypes.GenerateGradient)
            {
                DrawGradient();
            }
            else if(_currentProcess == ProcessTypes.MultiplyTexture)
            {
                DrawMultiplyTextures();
            }
            else
            {
                DrawInputImageSection();
            }

            DrawOutputPreviewSection();
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
                case ProcessTypes.TiltShiftEffect:
                    DrawTiltShiftEffect();
                    break;
                case ProcessTypes.VignetteEfect:
                    DrawVignette();
                    break;
                case ProcessTypes.GenerateGradient:
                {
                    if (GUILayout.Button("Generate Gradient"))
                    {
                       previewImage =  GradientGenerator.GenerateGradientTexture(_intVal, _intVal, _gradientType, _gradient, _floatVal);
                    }
                }
                    break;
                case ProcessTypes.BorderTexture:
                    DrawBorder();
                    break;
                case ProcessTypes.SobelEdgeDetection:
                    DrawSobelEdgeDetection();
                    break;
                case ProcessTypes.RemovePixels:
                    DrawColorPaletteOption();
                    DrawRemovePixels();
                    break;
                case ProcessTypes.OutlineEffect:
                    DrawOutlineEffect();
                    break;
                case ProcessTypes.EdgeSmoothing:
                    DrawEdgeSmoothing();
                    break;
                case ProcessTypes.ResizeImage:
                    DrawResizeImage();
                    break;
                case ProcessTypes.GenerateColorPalette:
                    DrawColorPalette();
                    break;
                case ProcessTypes.AlphaGradient:
                    DrawAlphaGradient();
                    break;
            }
            if (GUILayout.Button("Accept As A New Input"))
            {
                inputImage = previewImage;
            }
            DrawExport();
        }

        private void DrawMultiplyTextures()
        {   
            GUILayout.BeginVertical(EditorStyles.helpBox);
            // Horizontal alignment for label and input field
            GUILayout.Label("Input Images", GUILayout.Width(80)); // Adjust the width as needed
            var width = Mathf.RoundToInt(imageWidth / 2);
           
            // Display Input Image
            GUILayout.BeginHorizontal();
            inputImage = (Texture2D)EditorGUILayout.ObjectField(inputImage, typeof(Texture2D), false, GUILayout.Width(width), GUILayout.Height(width));
            inputImage2 = (Texture2D)EditorGUILayout.ObjectField(inputImage2, typeof(Texture2D), false, GUILayout.Width(width), GUILayout.Height(width));
            //  GUILayout.Box(inputImage, GUILayout.Width(imageWidth), GUILayout.Height(imageWidth)); 
            GUILayout.EndHorizontal();
            if(GUILayout.Button("Multiply Images"))
                previewImage = TextureMultiplier.MultiplyTextures(inputImage, inputImage2);
            GUILayout.EndVertical();
        }
        private void DrawAlphaGradient()
        {   
            _gradient = EditorGUILayout.GradientField("Alpha Gradient", _gradient , GUILayout.Width(200));
            DrawFloat(0 , 360, "Gradient angle");
            _gradientType = (GradientGenerator.GradientType)EditorGUILayout.EnumPopup("Select Option:", _gradientType);
            if (GUILayout.Button("Generate Alpha Texture"))
            {
                previewImage = AlphaGradientGenerator.AdjustAlpha(inputImage, _gradientType, _gradient , _floatVal);
            }
        }
        private void DrawColorPalette()
        {
           _colorPaletteName =  EditorGUILayout.TextField("Name your palette", _colorPaletteName);
            DrawFloat(0.05f , 0.5f ,"Color Detection Sensitivity");
            if (GUILayout.Button("Generate  Color Palette"))
            {
                _colorsInPalette = ColorPaletteGenerator.GeneratePalette(inputImage , _floatVal);
            }
            
            if (GUILayout.Button("Save Palette"))
            {   _colorsInPalette = ColorPaletteGenerator.GeneratePalette(inputImage , _floatVal);
                var val = _colorPaletteHolder.ColorPalettes.FirstOrDefault(x => x.Id == _colorPaletteName);
                if (val != null)
                {
                    _colorPaletteHolder.ColorPalettes.Remove(val);
                }
                var colors = new List<Color>();
                foreach (var color in _colorsInPalette)
                {
                    colors.Add(color);
                }
                _colorPaletteHolder.ColorPalettes.Add(new ColorPalette(colors , _colorPaletteName));
                
            }
            GUILayout.Label("Colors Founded");
            _scrollPosition2 = EditorGUILayout.BeginScrollView(_scrollPosition2, GUILayout.Height(200));
            foreach (var color in _colorsInPalette)
            {
                EditorGUILayout.ColorField(color);
            }
            GUILayout.EndScrollView();
        }
        private void DrawResizeImage()
        {   
            DrawInt(4096 ,32, "Width");
            _intVal2 = EditorGUILayout.IntField("Height", _intVal2);
            _intVal2 = Mathf.Clamp(_intVal2, 32, 4096);
            _boolVal = EditorGUILayout.Toggle("Preserve Aspect Ratio"  , _boolVal);
            if (GUILayout.Button("ResizeImage"))
            {
                previewImage = TextureResizer.ResizeTexture(inputImage, _intVal, _intVal2 , _boolVal);
            }
        }
        private void DrawEdgeSmoothing()
        {   
            DrawInt(20 , 1 ,"Edge ThreshHold");
            if (GUILayout.Button("Smooth Edges"))
                previewImage = EdgeSmoothing.SmoothEdges(inputImage, _intVal);
        }
        private void DrawOutlineEffect()
        {   
            GUILayout.BeginHorizontal();
            DrawInt(5 , 1 , "Outline Thickness");
            _intVal2 = EditorGUILayout.IntField("Smoothnes Iteration" , _intVal2);
            _intVal2 = Mathf.Clamp(_intVal2, 1, 9);
            //_intVal2 = Mathf.Clamp(_intVal2, 1, 20);
            _colorVal = EditorGUILayout.ColorField("Outline Color", _colorVal);
            _colorVal.a = 255;
            _boolVal = EditorGUILayout.Toggle("Preserve Image", _boolVal);
            GUILayout.EndHorizontal();
            
            if(GUILayout.Button("Create Outline"))
                previewImage = OutlineEffect.CreateOutlineTexture(inputImage,_boolVal, _colorVal , _intVal , _intVal2 );
        }
        private void DrawRemovePixels()
        {   
            DrawFloat(0F , 10F , "SimilarityTreshHold");
            if (GUILayout.Button("Remove Selected Colors"))
            {   
                previewImage = RemovePixels.RemoveSimilarPixelsByColorPalette(inputImage, _colorsInPalette.ToArray() , _floatVal);
            }
        }
        private void DrawSobelEdgeDetection()
        {    
            DrawFloat(0 , 10 , "Edge ThreshHold");
            DrawInt(10 , 0 ,"Dilation Size");

            if (GUILayout.Button("Execute"))
            {
                var test =  ImageProcessingUtils.Outline(inputImage, _floatVal, _intVal);
                previewImage = RemoveBackground.RemoveBackGroud(inputImage);
            }
        }
        private void DrawBorder()
        {   
            GUILayout.BeginHorizontal();
            DrawInt(20 , 0 , "Border Thickness");
            _colorVal = EditorGUILayout.ColorField("Border Color",_colorVal);
            _colorVal2 = EditorGUILayout.ColorField("Shadow Color", _colorVal2);
            DrawFloat(0 , 20 ,"Alpha Threshhold");
            _floatVal2 = EditorGUILayout.FloatField("ShadowBlurStrength", _floatVal2);
            _floatVal2 = Mathf.Clamp(_floatVal2 ,0f, 20f);
            _floatVal3 = EditorGUILayout.FloatField("Shadow Trancperency"  ,_floatVal3 );
            GUILayout.EndHorizontal();
            if(GUILayout.Button("Generate Border"))
                previewImage = BorderTextureGenerator.GenerateBorderTexture(inputImage, _intVal, _colorVal, _colorVal2,
                _floatVal, _floatVal2, _floatVal3);
        }
        private void DrawGradient()
        {   
           GUILayout.BeginVertical(EditorStyles.helpBox , GUILayout.Width(300));
           GUILayout.Label("Gradient Parameters", GUILayout.Width(160)); // Adjust the width as needed
            _gradient = EditorGUILayout.GradientField(_gradient , GUILayout.Width(200));
            DrawInt(2048 , 32 , "outputsize");
            DrawFloat(0 , 360, "Gradient angle");
            _gradientType = (GradientGenerator.GradientType)EditorGUILayout.EnumPopup("Select Option:", _gradientType);
           GUILayout.EndVertical();
            
        }
        private void DrawVignette()
        {
            
        }
        private void DrawTiltShiftEffect()
        {   
          
            DrawInt(20 , 0 , "blur amount");
            if (GUILayout.Button("Tilt Shift"))
            {
               previewImage =  TiltShiftEffect.ApplyTiltShiftEffect(inputImage, _intVal);
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
                color = EditorGUI.ColorField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), color);
                colors[index] = color;
                
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
            GUILayout.Label("Color Palette", EditorStyles.boldLabel);
            _boolVal = EditorGUILayout.Toggle("Use Your Palette", _boolVal);
          
            // Dropdown button for color selection
            GUILayout.BeginVertical();
           _scrollPosition2 =  GUILayout.BeginScrollView(_scrollPosition2, GUILayout.Height(100F));
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
                    EditorGUILayout.ColorField(item );
                }    
            }
            else
            {
                colorList.DoLayoutList();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
           
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
            GUILayout.BeginVertical();
            DrawInt(50 , 1, "Pixel Size");
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
            GUILayout.EndVertical();
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
            GUILayout.BeginVertical(EditorStyles.helpBox , GUILayout.Width(400F));

            GUILayout.Label("Operations", EditorStyles.boldLabel);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition  ,GUILayout.Height(400f) , GUILayout.Width(400F) );

            // Draw buttons inside the scroll view
            DrawButton("Convert to PixelArt", ProcessTypes.ConvertToPixelArt);
            DrawButton("Remap Texture", ProcessTypes.RemapTexture);
            DrawButton("Convert to Gray Scale", ProcessTypes.ConvertToGrayScale);
            DrawButton("AdjustBrightness", ProcessTypes.AdjustBrightness);
            DrawButton("Darken Texture", ProcessTypes.DarkenTexture);
            DrawButton("Invert Colors", ProcessTypes.InvertColors);
            DrawButton("Sketch", ProcessTypes.Sketch);
            DrawButton("Adjust Saturation", ProcessTypes.SaturationAdjustment);
            DrawButton("Posterize", ProcessTypes.Posterize);
            DrawButton("Change Contrast", ProcessTypes.AdjustContrast);
            DrawButton("ThreshHold Texture", ProcessTypes.Threshold);
            DrawButton("Sharpen Texture", ProcessTypes.SharpenTexture);
            DrawButton("Solarize Effect", ProcessTypes.SolarizeEffect);
            DrawButton("Apply Blur", ProcessTypes.BlurTexture);
            DrawButton("Duotone Effect", ProcessTypes.DuotoneEffect);
            DrawButton("Emboss Effect", ProcessTypes.EmbossEffect);
            DrawButton("Glow Effect", ProcessTypes.GlowEffect);
            DrawButton("Hue Displacement", ProcessTypes.HueDisplacement);
            DrawButton("Add Noise", ProcessTypes.NoiseGenerator);
            DrawButton("TiltShiftEffect", ProcessTypes.TiltShiftEffect);
            DrawButton("Generate Gradient", ProcessTypes.GenerateGradient);
            DrawButton("Generate Border" , ProcessTypes.BorderTexture);
            DrawButton("Remove Colors" , ProcessTypes.RemovePixels);
            DrawButton("Outline Effect" , ProcessTypes.OutlineEffect);
            DrawButton("Edge Smoothing" , ProcessTypes.EdgeSmoothing);
            DrawButton("Resize Image" , ProcessTypes.ResizeImage);
            DrawButton("Generate ColorPalette" , ProcessTypes.GenerateColorPalette);
            DrawButton("Alpha Gradient" , ProcessTypes.AlphaGradient);
            DrawButton("Multiply Images" , ProcessTypes.MultiplyTexture);

            // End the scroll view
           GUILayout.EndScrollView();
            GUILayout.EndVertical();
           
        }
        
        private void DrawButton(string label, ProcessTypes processType)
        {
            // Create a GUIStyle for selected and unselected buttons
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button  );
            if (_currentProcess == processType)
            {
                buttonStyle.normal.textColor = Color.green; // Change the text color for the selected button
            }

            if (GUILayout.Button(label, buttonStyle , GUILayout.Width(400)))
            {
                _currentProcess = processType;
            }
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
            GUILayout.Box(previewImage  ,GUILayout.Width(imageWidth), GUILayout.Height(imageWidth) );
            //var temp = (Texture2D)EditorGUILayout.ObjectField(inputImage, typeof(Texture2D), false, GUILayout.Width(imageWidth), GUILayout.Height(imageWidth));
            GUILayout.EndVertical();
        }
        
        private void DrawExport()
        {
            if (GUILayout.Button("Export as png"))
            {
                string path = EditorUtility.SaveFilePanel("Save Image", "", "NewImage", "png");
                byte[] bytes =previewImage.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
                AssetDatabase.Refresh();
            }
        }
    }
        
}
