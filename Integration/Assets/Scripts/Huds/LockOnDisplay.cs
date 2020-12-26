using Assets.Scripts.Huds.Transitions;
using Assets.Scripts.Players.LockOns;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds
{
    public class LockOnDisplay : MonoBehaviour, ILockOnNotifiable
    {
        // -- Editor

        [Header("Values")]
        [Tooltip("Angular speed of the hint circle (degree per second).")]
        public float inSightAngularSpeed = 1.25f;

        [Tooltip("Angular speed of the lock circle (degree per second).")]
        public float lockedAngularSpeed = 2f;
        
        [Tooltip("Time to fade in/fade out (second).")]
        public float fadeDuration = 0.25f;

        [Tooltip("Multiplier applied on the object scale for the transition (scalar).")]
        public float scaleMultiplier = 1.5f;

        [Tooltip("Time to fade out on lock break (second).")]
        public float breakFadeOutDuration = 1f;

        [Tooltip("Strength of the shaking effect on lock break (unknown unit).")]
        public float breakShakeStrength = 100f;

        [Tooltip("Number of time per second the shake effect will occur on lock break (hertz).")]
        public int breakShakeFrequency = 30;


        [Header("Parts")]
        public Image lockCircle;
        public Image hintCircle;
        
        [Header("References")]
        public LockOnManager lockOnManager;

        // -- Class

        // lock
        private ShakeOutFadeTransition _lockCircleTransition;
        private FadeTransition _hintCircleTransition;

        void Start()
        {
            _lockCircleTransition = new ShakeOutFadeTransition(lockCircle, scaleMultiplier, fadeDuration, breakFadeOutDuration, breakShakeStrength, breakShakeFrequency);
            _hintCircleTransition = new FadeTransition(hintCircle, scaleMultiplier, fadeDuration);

            _lockCircleTransition.Initialize();
            _hintCircleTransition.Initialize();
        }

        // Update Priority: +100
        // Ensure target viewport position is not a frame behind
        void LateUpdate()
        {
            if (lockOnManager.HasTargetLocked)
            {
                lockCircle.transform.Rotate(lockCircle.transform.forward, lockedAngularSpeed);

                Vector2 viewportPosition = lockOnManager.TargetViewportPosition;
                Vector2 screenPosition = new Vector2(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(lockCircle.rectTransform.parent as RectTransform, screenPosition, cam: null, out Vector2 localPosition);
                lockCircle.transform.localPosition = localPosition;
            }
            else if (lockOnManager.HasAnyTargetInSight)
            {
                Vector2 viewportPosition = lockOnManager.TargetViewportPosition;
                Vector2 screenPosition = new Vector2(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(hintCircle.rectTransform.parent as RectTransform, screenPosition, cam:null, out Vector2 localPosition);
                hintCircle.transform.localPosition = localPosition;

                hintCircle.transform.Rotate(hintCircle.transform.forward, inSightAngularSpeed);
            }
        }

        public void OnLockOn()
        {
            hintCircle.gameObject.SetActive(false);
            _lockCircleTransition.FadeIn();
        }

        public void OnUnlock()
        {
            hintCircle.gameObject.SetActive(true);
            _lockCircleTransition.FadeOut();
        }
        
        public void OnLockBreak()
        {
            hintCircle.gameObject.SetActive(true);
            _lockCircleTransition.ShakeOut();
        }

        public void OnLockableInSight()
        {
            _hintCircleTransition.FadeIn();
        }

        public void OnLockableOutOfSight()
        {
            _hintCircleTransition.FadeOut();
        }
    }
}