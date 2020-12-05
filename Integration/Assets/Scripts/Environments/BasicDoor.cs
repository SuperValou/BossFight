using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class BasicDoor : Door
    {
        private Animator _animator;
        private const string OpenAnimTrigger = "Open";
        private const string CloseAnimTrigger = "Close";

        protected override void Start()
        {
            base.Start();
            _animator = this.GetOrThrow<Animator>();
        }

        protected override void OnLoading(float progress)
        {
            // do nothing
        }

        protected override void OnOpen()
        {
            _animator.SetTrigger(OpenAnimTrigger);
            _animator.ResetTrigger(CloseAnimTrigger);
        }

        protected override void OnClose()
        {
            _animator.SetTrigger(CloseAnimTrigger);
            _animator.ResetTrigger(OpenAnimTrigger);
        }
    }
}