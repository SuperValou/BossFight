using Assets.Scripts.Cutscenes;
using Assets.Scripts.Damages;
using Assets.Scripts.Foes.ArtificialIntelligences;
using Assets.Scripts.Foes.Strikers.StrikerAi;
using Assets.Scripts.Huds;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Foes.Strikers
{
    public class Striker : Damageable, IStateMachine<StrikerBehaviour>
    {
        // -- Editor

        [Header("Parts")]
        public ShockWaveEmitter stompingAttack;

        [Header("References")]
        [Tooltip("Target of the " + nameof(Striker) + ". Can be null.")]
        public Transform target;

        [Tooltip("What to do when the " + nameof(Striker) + " dies.")]
        public BossDeath death;

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
            
        }

        public void SetCurrentBehaviour(StrikerBehaviour behaviour)
        {
            //Debug.Log($"Current behaviour: {behaviour.GetType().Name}");
        }

        public void OnStomping()
        {
            stompingAttack.EmitShockWave();
        }

        protected override void OnDamageTaken()
        {
            if (foeHealthDisplayProxy != null)
            {
                foeHealthDisplayProxy.Show((Damageable) this);
            }

            this.Animator.SetBool(StrikerAnimatorConstants.TargetIsInSightBool, true);
        }

        protected override void Die()
        {
            Animator.SetTrigger(StrikerAnimatorConstants.DeathTrigger);
            death?.Activate();
        }
    }
}