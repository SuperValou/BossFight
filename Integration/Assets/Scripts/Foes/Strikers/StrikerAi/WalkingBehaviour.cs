using Assets.Scripts.Foes.ArtificialIntelligences;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Foes.Strikers.StrikerAi
{
    public class WalkingBehaviour : Behaviour<Striker>
    {
        // -- Editor

        [Tooltip("Distance to the target (in meters) to reach before transitioning to another state.")]
        public float minDistanceToTarget = 10;

        // -- Class
        
        private Transform _target;
        private NavMeshAgent _navMeshAgent;

        private float _minSquaredDistanceToTarget; // avoid usage of Sqrt
        
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_target == null)
            {
                return;
            }

            _navMeshAgent.SetDestination(_target.position);

            float squaredDistanceToTarget = (_target.transform.position - StateMachine.transform.position).sqrMagnitude;
            if (squaredDistanceToTarget < _minSquaredDistanceToTarget)
            {
                animator.SetTrigger(StrikerAnimatorConstants.StompTrigger);
            }
        }
    }
}