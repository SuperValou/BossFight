using Assets.Scripts.Players;
using UnityEngine;

namespace Assets.Scripts.Cutscenes
{
    public class BossDeath : MonoBehaviour
    {
        // -- Editor
        public PlayerProxy playerProxy;

        // -- Class

        public void Activate()
        {
            playerProxy.SetInvulnerability(isInvulnerable: true);

        }
    }
}