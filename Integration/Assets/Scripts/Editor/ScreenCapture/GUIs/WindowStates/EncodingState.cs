using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Media;
using UnityEngine;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates
{
    public class EncodingState : IWindowState
    {
        private bool _isInitialized;
        private readonly string _imageSequenceFolder;
        private readonly int _fps;

        private VideoTrackAttributes _videoAttributes;

        public WindowStateName Name => WindowStateName.Encoding;

        public EncodingState(string imageSequenceFolder, int fps)
        {
            if (Directory.Exists(imageSequenceFolder))
            {
                throw new ArgumentException($"Directory '{nameof(imageSequenceFolder)}' doesn't exist at '{imageSequenceFolder}'.");
            }

            _imageSequenceFolder = imageSequenceFolder;
            
            if (fps < 1)
            {
                throw new ArgumentException($"{nameof(fps)} cannot be less than 1.");
            }

            _fps = fps;
        }

        public IWindowState OnGui()
        {
            if (!_isInitialized)
            {
                Initialize();
                return this;
            }

            throw new System.NotImplementedException();
        }

        private void Initialize()
        {
            string imagePath = Directory.EnumerateFiles(_imageSequenceFolder).First();

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            _videoAttributes = new VideoTrackAttributes
            {
                frameRate = new MediaRational(1, _fps),
                width = (uint) texture.width,
                height = (uint) texture.height,
                includeAlpha = false,
                bitRateMode = UnityEditor.VideoBitrateMode.High
            };

            //EditorCoroutineUtility.StartCoroutine(CountEditorUpdates(), this);
            //_isInitialized = true;
            //using (var encoder = new MediaEncoder(filePathMP4, videoAttr))
            //{
            //    IEnumerable<Texture2D> textures = new List<Texture2D>();
            //    foreach (var tex in textures)
            //        encoder.AddFrame(tex);
            //}
        }
    }
}