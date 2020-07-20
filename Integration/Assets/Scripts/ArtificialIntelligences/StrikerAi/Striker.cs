using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.ArtificialIntelligences.StrikerAi
{
    public class Striker : MonoBehaviour, IStateMachine<StrikerBehaviour>
    {
        [Header("References")]
        public Transform target;

        private Animator _animator;
        private StrikerBehaviour[] _behaviours;

        private NavMeshAgent _navMeshAgent;
        
        void Start()
        {
            _animator = this.GetOrThrow<Animator>();
            _behaviours = _animator.GetBehaviours<StrikerBehaviour>();

            foreach (var strikerBehaviour in _behaviours)
            {
                strikerBehaviour.Initialize(this);
            }

            _navMeshAgent = this.GetOrThrow<NavMeshAgent>();
        }

        void Update()
        {
            FaceTarget();
        }

        private void FaceTarget()
        {
            if (target == null)
            {
                return;
            }

            _navMeshAgent.SetDestination(target.position);
        }

        public void SetCurrentBehaviour(StrikerBehaviour behaviour)
        {
            Debug.Log($"Current behaviour: {behaviour.GetType().Name}");
        }
    }
}