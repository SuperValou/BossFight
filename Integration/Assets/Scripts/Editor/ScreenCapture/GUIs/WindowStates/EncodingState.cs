using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Media;
using UnityEngine;

namespace Assets.Scripts.Editor.ScreenCapture.GUIs.WindowStates
{
    public class EncodingState : IWindowState
    {
        private readonly string _imageSequenceFolder;
        private readonly int _fps;

        private readonly object _lock = new object();

        private bool _isInitialized = false;
        private VideoTrackAttributes _videoAttributes;
        
        private Task _encodingTask;
        private string _encodingMessage;

        public WindowStateName Name => WindowStateName.Encoding;

        public EncodingState(string imageSequenceFolder, int fps)
        {
            if (!Directory.Exists(imageSequenceFolder))
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

            lock (_lock)
            {
                EditorGUILayout.LabelField(_encodingMessage);
            }
            
            return this;
        }

        private void Initialize()
        {
            string imagePath = Directory.EnumerateFiles(_imageSequenceFolder).FirstOrDefault();
            if (imagePath == null)
            {
                throw new ArgumentException($"Frames folder is empty: {_imageSequenceFolder}");
            }

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

            _encodingTask = Task.Run((Action) Encode);
            _isInitialized = true;
        }

        private void Encode()
        {
            lock (_lock)
            {
                _encodingMessage = "Checking output...";
            }

            string outputFilePath = Path.Combine(_imageSequenceFolder, "gameplay.mp4");
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            lock (_lock)
            {
                _encodingMessage = "Counting frames...";
            }

            var framePaths = Directory.GetFiles(_imageSequenceFolder);
            
            using (var encoder = new MediaEncoder(outputFilePath, _videoAttributes))
            {
                for (int i = 0; i < framePaths.Length; i++)
                {
                    lock (_lock)
                    {
                        _encodingMessage = $"Encoding frame {i}/{framePaths.Length}...";
                    }

                    string framePath = framePaths[i];
                    byte[] imageBytes = File.ReadAllBytes(framePath);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageBytes);

                    encoder.AddFrame(texture);
                }
            }

            lock (_lock)
            {
                _encodingMessage = $"Done: {outputFilePath}";
            }
        }
    }
}