using System;
using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences
{
    public abstract class Behaviour<TStateMachine> : StateMachineBehaviour, IBehaviour<TStateMachine>
        where TStateMachine : class, IStateMachine
    {
        protected TStateMachine StateMachine { get; private set; }

        public void Initialize(TStateMachine stateMachine)
        {
            StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }
    }
}