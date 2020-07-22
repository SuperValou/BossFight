using Assets.Scripts.ArtificialIntelligences;
using Assets.Scripts.Damages;
using Assets.Scripts.Foes.Strikers.StrikerAi;
using Assets.Scripts.Proxies;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Foes.Strikers
{
    public class Striker : Damageable, IStateMachine<StrikerBehaviour>
    {
        // -- Editor

        [Header("Parts")]
        public DamagingParticleSystem stompingAttack;

        [Header("References")]
        [Tooltip("Target of the " + nameof(Striker) + ". Can be null.")]
        public Transform target;

        [Tooltip("System displaying the "+ nameof(Striker) + "'s health on screen. Can be null.")]
        public FoeHealthDisplayProxy foeHealthDisplayProxy;

        // -- Class

        private StrikerBehaviour[] _behaviours;

        internal Transform Target => target;
        internal Animator Animator { get; private set; }
        internal NavMeshAgent NavMeshAgent { get; private set; }

        void Start()
        {
            Animator = this.GetOrThrow<Animator>();
            NavMeshAgent = this.GetOrThrow<NavMeshAgent>();

            _behaviours = Animator.GetBehaviours<StrikerBehaviour>();

            foreach (var strikerBehaviour in _behaviours)
            {
                strikerBehaviour.Initialize(stateMachine: this);
            }
        }

        void Update()
        {
            // Do not put anything in here, use "OnStateUpdate" method in StrikerBehaviours instead.
        }

        public void SetCurrentBehaviour(StrikerBehaviour behaviour)
        {
            //Debug.Log($"Current behaviour: {behaviour.GetType().Name}");
        }

        public void OnStomping()
        {
            stompingAttack.Execute();
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