using System;
using System.IO;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers
{
    public class SceneInfoSerializer
    {
        private const int Version = 2;

        public SceneInfo ReadFromFile(string filePath)
        {
            string serializedInfo = File.ReadAllText(filePath);
            SceneLinkFile linkFile = JsonUtility.FromJson<SceneLinkFile>(serializedInfo);
            if (linkFile.SerializationMajorVersion != Version)
            {
                throw new ArgumentException($"Incorrect version {linkFile.SerializationMajorVersion}. " +
                                            $"Only version {Version} is handled.");
            }

            return linkFile.SceneInfo;
        }

        public void WriteToFile(SceneInfo sceneInfo, string filePath)
        {
            SceneLinkFile linkFile = new SceneLinkFile()
            {
                SerializationMajorVersion = Version,
                SceneInfo = sceneInfo
            };

            string serializedInfo = JsonUtility.ToJson(linkFile, prettyPrint: true);
            File.WriteAllText(filePath, serializedInfo);
        }
    }
}