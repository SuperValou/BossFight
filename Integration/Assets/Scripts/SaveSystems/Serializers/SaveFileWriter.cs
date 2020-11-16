using System;

namespace Assets.Scripts.SaveSystems.Serializers
{
    public class SaveFileWriter
    {
        public string FilePath { get; }

        public SaveFileWriter(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Write()
        {

        }
    }
}