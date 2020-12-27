using System;
using Assets.Scripts.Foes.ArtificialIntelligences;
using Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Foes.Shells
{
    public class ShellAi : MonoBehaviour, IStateMachine
    {
        // -- Class

        private const string InitializedBool = "IsInitialized";
        private Animator _animator;

        void Start()
        {
            _animator = this.GetOrThrow<Animator>();
            var behaviours = _animator.GetBehaviours<Behaviour<ShellAi>>();
            foreach (var behaviour in behaviours)
            {
                behaviour.Initialize(stateMachine: this);
            }

            _animator.SetBool(InitializedBool, value: true);
        }
    }
}