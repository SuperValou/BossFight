using System.Collections.Generic;
using Assets.Scripts.Utilities.Editor.ScriptLinks.Serializers.DTOs;

namespace Assets.Scripts.Utilities.Editor.ScriptLinks
{
    public interface ISceneInfoManager
    {
        bool IsInitialized { get; }

        ICollection<SceneReport> Reports { get; }

        void Initialize();

        void TakeSnapshot();
    }
}