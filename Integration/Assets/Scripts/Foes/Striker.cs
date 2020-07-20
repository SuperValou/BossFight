using Assets.Scripts.Damages;
using Assets.Scripts.Huds;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.ArtificialIntelligences.StrikerAi
{
    public class Striker : Damageable, IStateMachine<StrikerBehaviour>
    {
        // -- Editor
        [Header("References")]
        [Tooltip("Target of the " + nameof(Striker) + ". Can be null.")]
        public Transform target;

        [Tooltip("System displaying the "+ nameof(Striker) + "'s health on screen. Can be null.")]
        public FoeHealthDisplayProxy foeHealthDisplayProxy;

        // -- Class

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

        protected override void OnDamageTaken()
        {
            if (foeHealthDisplayProxy != null)
            {
                foeHealthDisplayProxy.Show((Damageable) this);
            }
        }

        protected override void Die()
        {
            // Victory
            Debug.LogWarning("Victory!");
        }
    }
}