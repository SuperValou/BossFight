using Assets.Scripts.Cutscenes;
using Assets.Scripts.Damages;
using Assets.Scripts.Foes.ArtificialIntelligences;
using Assets.Scripts.Foes.Strikers.StrikerAi;
using Assets.Scripts.Utilities;
using Assets.Scripts.Weaponry.Projectiles;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Foes.Strikers
{
    public class Striker : Damageable, IStateMachine
    {
        // -- Editor

        [Header("Parts")]
        public ShockWaveEmitter stompingAttack;

        [Header("References")]
        [Tooltip("Target of the " + nameof(Striker) + ". Can be null.")]
        public Transform target;

        [Tooltip("What to do when the " + nameof(Striker) + " dies.")]
        public BossDeath death;
        
        // -- Class
        
        internal Animator Animator { get; private set; }
        internal NavMeshAgent NavMeshAgent { get; private set; }

        void Start()
        {
            Animator = this.GetOrThrow<Animator>();
            NavMeshAgent = this.GetOrThrow<NavMeshAgent>();

            var behaviours = Animator.GetBehaviours<Behaviour<Striker>>();

            foreach (var strikerBehaviour in behaviours)
            {
                strikerBehaviour.Initialize(stateMachine: this);
            }
        }

        void Update()
        {
            
        }

        public void OnStomping()
        {
            stompingAttack.EmitShockWave();
        }

        protected override void OnDamage(VulnerableCollider hitCollider, DamageData damageData, MonoBehaviour damager)
        {
            this.Animator.SetBool(StrikerAnimatorConstants.TargetIsInSightBool, true);
        }

        protected override void OnDeath()
        {
            Animator.SetTrigger(StrikerAnimatorConstants.DeathTrigger);
            death?.Activate();
        }
    }
}