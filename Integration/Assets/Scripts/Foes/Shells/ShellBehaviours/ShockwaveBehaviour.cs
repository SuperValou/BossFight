using Assets.Scripts.Foes.ArtificialIntelligences;
using UnityEngine;

namespace Assets.Scripts.Foes.Shells.ShellBehaviours
{
    public class ShockwaveBehaviour : Behaviour<ShellAi>
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            StateMachine.DoShockwaveAttack();
        }
    }
}