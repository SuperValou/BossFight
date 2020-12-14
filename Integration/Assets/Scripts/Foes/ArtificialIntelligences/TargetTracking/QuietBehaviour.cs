using UnityEngine;

namespace Assets.Scripts.Foes.ArtificialIntelligences.TargetTracking
{
    public class QuietBehaviour : TargetTrackingBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.OnBecomeQuiet();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.QuietUpdate();
        }
    }
}