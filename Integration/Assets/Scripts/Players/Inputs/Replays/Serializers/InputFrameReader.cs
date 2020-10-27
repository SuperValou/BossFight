using System;
using System.IO;
using Assets.Scripts.Players.Inputs.Replays.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays.Serializers
{
    public class InputFrameReader
    {
        private StreamReader _streamReader = null;
        private bool _isClosed = false;

        public string FilePath { get; }

        public InputFrameReader(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Open()
        {
            if (_streamReader != null)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} is already open.");
            }

            if (_isClosed)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} was closed and cannot be reopened.");
            }

            FileStream fileStream = File.OpenRead(FilePath);
            _streamReader = new StreamReader(fileStream);
            
            string line = _streamReader.ReadLine();
            RecordFileHeader fileHeader = JsonUtility.FromJson<RecordFileHeader>(line);
            if (fileHeader.SerializationMajorVersion != SerializationConstants.Version)
            {
                throw new ArgumentException($"Expected version {SerializationConstants.Version}, but found unsupported version {fileHeader.SerializationMajorVersion}.");
            }
        }

        public bool CanRead()
        {
            return _streamReader != null 
               && !_isClosed
               && !_streamReader.EndOfStream;
        }

        public InputFrame ReadFrame()
        {
            if (_streamReader == null)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} is not open. Did you forget to call the {nameof(Open)} method?");
            }

            if (_isClosed)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} was closed.");
            }
            
            string line = _streamReader.ReadLine();
            InputFrame frame = JsonUtility.FromJson<InputFrame>(line);

            if (_streamReader.EndOfStream)
            {
                Debug.LogWarning("End of replay");
                Close();
            }

            return frame;
        }

        public void Close()
        {
            _streamReader.Close();
            _isClosed = true;
        }
    }
}