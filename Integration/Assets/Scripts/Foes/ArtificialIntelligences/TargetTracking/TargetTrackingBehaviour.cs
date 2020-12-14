using System;
using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking
{
    public abstract class TargetTrackingBehaviour : StateMachineBehaviour, IBehaviour<ITargetTrackingStateMachine>
    {
        protected ITargetTrackingStateMachine StateMachine { get; private set; }

        public void Initialize(ITargetTrackingStateMachine stateMachine)
        {
            StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }
    }
}