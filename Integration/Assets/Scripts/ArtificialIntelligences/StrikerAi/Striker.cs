using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.ArtificialIntelligences.StrikerAi
{
    public class Striker : MonoBehaviour, IStateMachine<StrikerBehaviour>
    {
        [Header("Parts")]
        public Transform leftCanon;
        public Transform rightCanon;

        [Header("References")]
        public Transform target;

        private Animator _animator;
        private StrikerBehaviour[] _behaviours;
        
        void Start()
        {
            _animator = this.GetOrThrow<Animator>();
            _behaviours = _animator.GetBehaviours<StrikerBehaviour>();

            foreach (var strikerBehaviour in _behaviours)
            {
                strikerBehaviour.Initialize(this);
            }
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

            leftCanon.LookAt(target);
            leftCanon.Rotate(leftCanon.up, 90, Space.World);
            rightCanon.LookAt(target);
            rightCanon.Rotate(rightCanon.up, 90, Space.World);
        }

        public void SetCurrentBehaviour(StrikerBehaviour behaviour)
        {
            Debug.Log($"Current behaviour: {behaviour.GetType().Name}");
        }
    }
}