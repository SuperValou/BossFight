using Assets.Scripts.Players.LockOns;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds
{
    public class LockOnDisplay : MonoBehaviour, ILockOnNotifiable
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Angular speed of the lock circle (degree per second).")]
        public float angularSpeed = 2f;

        [Tooltip("Time to fade in on lock-on (seconds).")]
        public float lockOnFadeIn = 0.1f;

        [Tooltip("Time to fade out on lock break (seconds).")]
        public float lockBreakFadeOut = 0.1f;

        [Header("Parts")]
        public Image lockCircle;

        [Header("References")]
        public LockOnManager lockOnManager;

        // -- Class

        private Color lockCircleInitialColor;
        private Color fadedOutLockCircleColor;

        void Start()
        {
            lockCircleInitialColor = lockCircle.color;
            fadedOutLockCircleColor = new Color(lockCircleInitialColor.r, lockCircleInitialColor.g, lockCircleInitialColor.b, a: 0);
            lockCircle.gameObject.SetActive(false);
        }

        void Update()
        {
            if (lockOnManager.IsLocked)
            {
                lockCircle.transform.Rotate(lockCircle.transform.forward, angularSpeed);
            }
        }

        public void OnLockOn()
        {
            lockCircle.color = fadedOutLockCircleColor;
            lockCircle.gameObject.SetActive(true);
            lockCircle.DOColor(lockCircleInitialColor, lockOnFadeIn);
        }

        public void OnLockBreak()
        {
            var tween = lockCircle.DOColor(fadedOutLockCircleColor, lockBreakFadeOut);
            tween.OnComplete(() => lockCircle.gameObject.SetActive(false));
        }

        public void OnUnlock()
        {
            lockCircle.gameObject.SetActive(false);
        }
    }
}