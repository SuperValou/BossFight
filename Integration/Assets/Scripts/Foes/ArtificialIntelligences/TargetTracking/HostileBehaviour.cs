using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking
{
    public class HostileBehaviour : Behaviour<ITargetTrackingStateMachine>
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.OnBecomeHostile();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.HostileUpdate();
        }
    }
}