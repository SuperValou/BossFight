using Assets.Scripts.Foes.ArtificialIntelligences;
using UnityEngine;

namespace Assets.Scripts.Foes.Shells.ShellBehaviours
{
    public class IdleBehaviour : Behaviour<ShellAi>
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.OnIdle();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.IdleUpdate();
        }
    }
}