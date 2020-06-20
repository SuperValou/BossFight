using System;
using UnityEngine;

namespace Assets.Scripts.ArtificialIntelligences.StrikerAi
{
    public abstract class StrikerBehaviour : StateMachineBehaviour, IBehaviour<Striker>
    {
        protected Striker Striker;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Striker.SetCurrentBehaviour(this);
        }

        public void Initialize(Striker stateMachine)
        {
            Striker = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }
    }
}