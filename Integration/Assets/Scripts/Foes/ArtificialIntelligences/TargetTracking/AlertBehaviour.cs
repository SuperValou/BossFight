using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking
{
    public class AlertBehaviour : Behaviour<ITargetTrackingStateMachine>
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.OnBecomeAlert();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.AlertUpdate();
        }
    }
}