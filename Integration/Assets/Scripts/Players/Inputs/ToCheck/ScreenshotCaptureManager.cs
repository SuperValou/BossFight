using System;
using System.IO;
using UnityEditor.Media;
using UnityEngine;

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
		    throw new FileNotFoundException($"Screenshot folder not found at '{outputFolder}'. Capture won't work.");
		}

		_outputFolder = outputFolder; // TODO: append build number
	    _isCapturing = captureRequired;
	    _elapsedTime = 0;
	}

    void FixedUpdate()
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
                _elapsedTime = 0;
				CaptureScreenshot();
            }

            return;
        }

        if (_isCapturing && !captureRequired)
        {
            _isCapturing = false;
			return;
        }

        _elapsedTime += Time.fixedDeltaTime;
        if (_elapsedTime < _frameDuration)
        {
			return;
        }

		CaptureScreenshot();
	}

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        // TODO: generate video
        //VideoTrackAttributes videoAttr = new VideoTrackAttributes
        //{
        //    frameRate = new MediaRational(1, fps),
        //    width = (uint) Width,
        //    height = (uint) Height,
        //    includeAlpha = false,
        //    bitRateMode = UnityEditor.VideoBitrateMode.High
        //};

        //using (var encoder = new MediaEncoder(filePathMP4, videoAttr))
        //    foreach (var tex in textures)
        //        encoder.AddFrame(tex);
        
    }
#endif
    

    private void CaptureScreenshot()
    {
        string screenshotFilename = $"frame_{Time.frameCount:D04}.png";
        string screenshotFullPath = Path.Combine(_outputFolder, screenshotFilename);
        ScreenCapture.CaptureScreenshot(screenshotFullPath);
    }
}