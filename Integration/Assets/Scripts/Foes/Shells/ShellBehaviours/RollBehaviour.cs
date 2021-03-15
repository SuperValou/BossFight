using Assets.Scripts.Foes.ArtificialIntelligences;
using UnityEngine;

namespace Assets.Scripts.Foes.Shells.ShellBehaviours
{
    public class RollBehaviour : Behaviour<ShellAi>
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.OnRoll();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.RollUpdate();
        }
    }
}