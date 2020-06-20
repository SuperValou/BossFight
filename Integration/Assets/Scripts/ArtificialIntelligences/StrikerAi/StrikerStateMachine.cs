using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.ArtificialIntelligences.StrikerAi
{
    public class StrikerStateMachine : MonoBehaviour, IStateMachine<StrikerBehaviour>
    {
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
        
        public void SetCurrentBehaviour(StrikerBehaviour behaviour)
        {
            Debug.Log($"Current behaviour: {behaviour.GetType().Name}");
        }
    }
}