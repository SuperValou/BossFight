using System;
using Assets.Scripts.Foes.ArtificialIntelligences;
using UnityEngine;

namespace Assets.Scripts.Foes.Strikers.StrikerAi
{
    /// <summary>
    /// Base class for all <see cref="Striker"/> behaviours.
    /// </summary>
    public abstract class StrikerBehaviour : StateMachineBehaviour, IBehaviour<Striker>
    {
        protected Striker Striker { get; private set; }
        protected  Animator Animator { get; private set; }

        public virtual void Initialize(Striker stateMachine)
        {
            Striker = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            Animator = Striker.Animator;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Striker.SetCurrentBehaviour(this);
        }
    }
}