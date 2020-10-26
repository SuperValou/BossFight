using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public class ScriptLinkManager
    {
        private const string SclFileExtension = ".scl";

        public bool IsInitialized { get; private set; } = false;

        public ICollection<SceneReport> Reports { get; } = new List<SceneReport>();

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.IsValid())
                {
                    throw new ArgumentException($"Scene '{scene.name}' is invalid.");
                }

                if (scene.isDirty)
                {
                    throw new ArgumentException($"Scene '{scene.name}' must be saved first.");
                }

                SceneReport report = new SceneReport(scene);
                Reports.Add(report);
            }

            foreach (var report in Reports)
            {
                report.Build();
            }

            IsInitialized = true;
        }

        public void Clear()
        {
            this.Reports.Clear();
            IsInitialized = false;
        }

        public void TakeSnapshot()
        {
            if (Reports.Count == 0)
            {
                throw new InvalidOperationException($"Nothing to snapshot yet. Call {nameof(Initialize)} before.");
            }

            foreach (var report in Reports)
            {
                if (report.MissingScriptPaths.Count > 0)
                {
                    Debug.LogWarning($"Skipping '{report.SceneName}' because it has missing scripts.");
                    continue;
                }

                var sceneInfo = report.SceneInfo;
                if (sceneInfo == null)
                {
                    Debug.LogError($"Skipping '{report.SceneName}' because it has a null {nameof(report.SceneInfo)} property.");
                    continue;
                }

                string sclFileRelativePath = Path.ChangeExtension(sceneInfo.ScenePath, SclFileExtension);
                string projectFolder = Application.dataPath.Remove(Application.dataPath.Length - "/Assets".Length);
                string sclFileAbsolutePath = Path.Combine(projectFolder, sclFileRelativePath);

                SceneInfoSerializer serializer = new SceneInfoSerializer();
                serializer.WriteToFile(sceneInfo, sclFileAbsolutePath);

                Debug.Log($"Snapshot available at '{sclFileAbsolutePath}'.");
            }
        }
    }
}
