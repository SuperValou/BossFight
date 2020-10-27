using System;
using System.IO;
using Assets.Scripts.Players.Inputs.Replays.Serializers.DTOs;
using UnityEngine;

namespace Assets.Scripts.Players.Inputs.Replays.Serializers
{
    public class InputFrameWriter
    {
        private StreamWriter _streamWriter = null;
        private bool _isClosed = false;

        public string FilePath { get; }

        public InputFrameWriter(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Open()
        {
            if (_streamWriter != null)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} is already open.");
            }

            if (_isClosed)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} was closed and cannot be reopened.");
            }

            FileStream fileStream = File.Open(FilePath, FileMode.Create);
            _streamWriter = new StreamWriter(fileStream);

            var header = new RecordFileHeader()
            {
                SerializationMajorVersion = SerializationConstants.Version
            };

            string serializedHeader = JsonUtility.ToJson(header);
            _streamWriter.WriteLine(serializedHeader);
        }

        public void WriteFrame(InputFrame frame)
        {
            if (_streamWriter == null)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} is not open. Did you forget to call the {nameof(Open)} method?");
            }

            if (_isClosed)
            {
                throw new ArgumentException($"{nameof(InputFrameWriter)} was closed.");
            }

            string serializedFrame = JsonUtility.ToJson(frame);
            _streamWriter.WriteLine(serializedFrame);
        }

        public void Close()
        {
            _streamWriter.Close();
            _isClosed = true;
        }
    }
}