using System;
using UnityEngine;

namespace Assets.Scripts.ArtificialIntelligences.StrikerAi
{
    public abstract class StrikerBehaviour : StateMachineBehaviour, IBehaviour<StrikerStateMachine>
    {
        protected StrikerStateMachine StateMachine;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.SetCurrentBehaviour(this);
        }

        public void Initialize(StrikerStateMachine stateMachine)
        {
            StateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }
    }
}