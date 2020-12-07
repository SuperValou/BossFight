using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays
{
    public class ScreenshotCaptureManager : MonoBehaviour
    {
        // -- Editor

        public int fps = 25;

        public bool captureRequired = false;
        public string outputFolder = "<to set>";

        // -- Class
	
        private string _outputFolder;
        private float _frameDuration;

        private bool _isCapturing;
        private float _elapsedTime;

        void Start()
        {
            if (fps < 1)
            {
                throw new ArgumentException($"{nameof(fps)} cannot be less than 1.");
            }

            _frameDuration = 1f / fps;

            if (!Directory.Exists(outputFolder))
            {
                throw new DirectoryNotFoundException($"Screenshot folder not found at '{outputFolder}'. Capture won't work.");
            }

            _outputFolder = outputFolder; // TODO: append build number
            _isCapturing = captureRequired;
            _elapsedTime = 0;
        }

        void Update()
        {
            if (_outputFolder == null)
            {
                return;
            }

            if (!_isCapturing)
            {
                if (captureRequired)
                {
                    _isCapturing = true;
                    CaptureScreenshot();
                    _elapsedTime = 0;
                }

                return;
            }

            if (_isCapturing && !captureRequired)
            {
                _isCapturing = false;
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime < _frameDuration)
            {
                return;
            }

            CaptureScreenshot();
            _elapsedTime = 0;
        }
    
        private void CaptureScreenshot()
        {
            string screenshotFilename = $"frame_{Time.frameCount:D04}.png";
            string screenshotFullPath = Path.Combine(_outputFolder, screenshotFilename);
            
            ScreenCapture.CaptureScreenshot(screenshotFullPath);
            Debug.Log($"Captured frame {Time.frameCount} after {_elapsedTime:0.###}s (~{(1f / _elapsedTime):0} fps)");
        }
    }
}