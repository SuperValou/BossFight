using Assets.Scripts.LoadingSystems.Doors;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class BasicDoor : Door
    {
        private Animator _animator;

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
            _animator.SetTrigger("Open");
        }

        protected override void OnClose()
        {
            _animator.SetTrigger("Close");
        }
    }
}