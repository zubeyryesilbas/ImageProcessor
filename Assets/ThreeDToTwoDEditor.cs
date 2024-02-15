using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using PixerEditor;

public class ThreeDToTwoDEditor : EditorWindow
{
    private Animator _animator;
    private Camera _camera;
    private int _captureSize = 256;
    private float _captureInterval = 1F / 30F;
    private int _selectedAnimationIndex = 0;
    private AnimationClip[] animationClips;
    private Texture2D _input;
    private Texture2D _output;
    private bool _animHasEnded = true;
    private FrameCapture _frameCapture;
    private List<Texture2D> _inputTextures = new List<Texture2D>();
    private List<Texture2D> _outPutTextures = new List<Texture2D>();
    private bool _canProcess = true;
    private float _timer = 0f;

    [MenuItem("Window/3D to 2D")]
    public static void ShowWindow()
    {
        GetWindow<ThreeDToTwoDEditor>("3D to 2D");
    }

   

    void OnGUI()
    {
        if (_frameCapture == null)
            _frameCapture = new FrameCapture();
        if (_input == null)
          _input = new Texture2D(512, 512, TextureFormat.RGB24, false);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Select a GameObject with an Animator:");
        _animator = EditorGUILayout.ObjectField(_animator, typeof(Animator), true) as Animator;
        _camera = EditorGUILayout.ObjectField(_camera , typeof(Camera) , true)as Camera;
        GUILayout.BeginVertical();
        _captureSize = EditorGUILayout.IntField("Caputure Size", _captureSize);
        _captureInterval = EditorGUILayout.FloatField("Capture Interval", _captureInterval);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if (_animator != null)
        {
            // Get the AnimatorController
            RuntimeAnimatorController controller = _animator.runtimeAnimatorController as RuntimeAnimatorController;

            if (controller != null)
            {
                // Get list of animation clips
                animationClips = controller.animationClips;
                // Dropdown to select animation
                GUILayout.Label("Select an animation:");
                _selectedAnimationIndex = EditorGUILayout.Popup(_selectedAnimationIndex, GetAnimationNames());
                // Play button
                // Play button
                if (_camera != null && _animator != null)
                {
                   
                    if (GUILayout.Button("Capture"))
                    {   
                       // EditorGUI.BeginChangeCheck();
                        PlayAnimation();
                        _frameCapture.Capture( _camera , 1f );
                       // EditorGUI.EndChangeCheck();
                    }
                }
            }
            else
            {
                GUILayout.Label("No Animator Controller attached to the Animator component.");
            }
        }
        else
        {
            GUILayout.Label("No GameObject selected or GameObject does not have an Animator component.");
        }
        Handletextures();
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        GUILayout.Label(_input.name);
        GUILayout.Box(_input, GUILayout.Width(512) , GUILayout.Height(512));
        EditorGUI.EndChangeCheck();
        GUILayout.Box(_output , GUILayout.Width(512) , GUILayout.Height(512));
        GUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
    }

    // Helper method to extract animation clip names
    private async void Handletextures()
    {
        if (_canProcess)
        {   
            _canProcess = false;
            var textures = Resources.LoadAll<Texture2D>("");
            int val = 0;
            var tempArray = new Texture2D[textures.Length];
            foreach (var item in textures)
            {
                if (int.TryParse(item.name.ToString(), out val))
                {
                    tempArray[val] = item;
                }
            }
            foreach (var  tex in tempArray)
            {
                EditorGUI.BeginChangeCheck();
                _input = tex;
                string assetPath = AssetDatabase.GetAssetPath(tex);

                // Modify the texture's import settings
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporter.isReadable = true; // Change isReadable to true or false as needed
                    AssetDatabase.ImportAsset(assetPath);
                   
                }
               _output = ImageToPixelArtConverter.ConvertToPixelArt(tex, 2, new Color[]
                {
                    Color.black, Color.cyan, Color.green, Color.red, Color.magenta, Color.yellow,Color.gray
                });
                EditorGUI.EndChangeCheck();
                Repaint(); 
                await Task.Delay(Mathf.RoundToInt(1000 * _captureInterval ));
            }
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>" + tempArray[9].name);
            _canProcess = true;
        }
        
    }
    string[] GetAnimationNames()
    {
        string[] animationNames = new string[animationClips.Length];
        for (int i = 0; i < animationClips.Length; i++)
        {
            animationNames[i] = animationClips[i].name;
        }
        return animationNames;
    }

    private async void PlayAnimation()
    {   
        if (_animHasEnded)
        {
            _animHasEnded = false;
            AnimationClip selectedClip = animationClips[0];
            var stateName = GetStringFromAniClip(_animator, animationClips[_selectedAnimationIndex]);
            var val = 0f;
            while (val <1.01f)
            {
                _animator.Play(stateName, -1, val);
                _animator.Update(val - 1);
                await Task.Delay(Mathf.RoundToInt(_captureInterval * 1000));
                val += _captureInterval;
            }
            _animHasEnded = true;
        }
       
    }
    string GetStringFromAniClip(Animator GetAnimator, AnimationClip Clip)
    {
        string OutString = "";
         var  AllState = GetAnimatorStates(GetAnimator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController); //모든 스테이트 가져오기   
        OutString = GetStateFromClip(AllState, Clip).name; //애니메이션 클립으로 부터 State가져 와서 이름 할당
        return OutString;
    }
    
    AnimatorState[] GetAnimatorStates(UnityEditor.Animations.AnimatorController anicon)
    {
        List<AnimatorState> ret = new List<AnimatorState>();
        foreach (var layer in anicon.layers)
        {
            foreach (var subsm in layer.stateMachine.stateMachines)
            {
                foreach (var state in subsm.stateMachine.states)
                {
                    ret.Add(state.state);
                }
            }
            foreach (var s in layer.stateMachine.states)
            {
                ret.Add(s.state);
            }
        }
        return ret.ToArray();
    }
    
    AnimatorState GetStateFromClip(AnimatorState[] StateList, AnimationClip GetClip)
    {
        AnimatorState OutState = null;
        for (int i = 0; i < StateList.Length; i++)
        {
            if (StateList[i].motion == GetClip)
            {
                OutState = StateList[i];
                break; //정지
            }
        }
        return OutState;
    }
}