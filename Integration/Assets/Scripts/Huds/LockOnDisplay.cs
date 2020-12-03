using Assets.Scripts.Players.LockOns;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds
{
    public class LockOnDisplay : MonoBehaviour, ILockOnNotifiable
    {
        // -- Editor

        [Header("Values - Locked Target")]
        [Tooltip("Angular speed of the lock circle (degree per second).")]
        public float lockedAngularSpeed = 2f;
        
        [Tooltip("Time to fade in/fade out when lock-in/locking-out (second).")]
        public float circleTweenTime = 0.25f;

        [Tooltip("Multiplier applied on the object scale for the transition (scalar).")]
        public float circleScaleMultiplier = 1.5f;

        [Tooltip("Time to fade out on lock break (second).")]
        public float breakCircleTweenTime = 1f;

        [Tooltip("Strength of the shaking effect on lock break (unknown unit).")]
        public float breakCircleShakeStrength = 100f;

        [Tooltip("Number of time per second the shake effect will occur on lock break (hertz).")]
        public int breakCircleShakeFrequency = 30;


        [Header("Values - Target in sight")]
        public float inSightAngularSpeed = 1.25f;


        [Header("Parts")]
        public Image lockCircle;
        public Image nearestTargetCircle;

        [Header("References")]
        public LockOnManager lockOnManager;

        // -- Class

        private Vector3 _lockCircleInitialPosition;
        private Vector3 _lockCircleInitialScale;
        private Color _lockCircleInitialColor;

        private Color _fadedOutLockCircleColor;
        private Vector3 _fadedOutLockCircleScale;

        


        private Tween _runningTween;

        void Start()
        {
            _lockCircleInitialPosition = lockCircle.transform.position;
            _lockCircleInitialScale = lockCircle.transform.localScale;
            _lockCircleInitialColor = lockCircle.color;
            
            _fadedOutLockCircleColor = new Color(_lockCircleInitialColor.r, _lockCircleInitialColor.g, _lockCircleInitialColor.b, a: 0);
            _fadedOutLockCircleScale = lockCircle.transform.localScale * circleScaleMultiplier;

            lockCircle.gameObject.SetActive(false);
        }

        void Update()
        {
            if (lockOnManager.HasTargetLocked)
            {
                lockCircle.transform.Rotate(lockCircle.transform.forward, lockedAngularSpeed);
            }

            if (lockOnManager.HasAnyTargetInSight)
            {
                nearestTargetCircle.gameObject.SetActive(true);

                Vector2 viewportPosition = lockOnManager.NearestTargetViewportPosition;
                Vector2 screenPosition = new Vector2(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(nearestTargetCircle.rectTransform.parent as RectTransform, screenPosition, cam:null, out Vector2 localPosition);
                nearestTargetCircle.transform.localPosition = localPosition;

                nearestTargetCircle.transform.Rotate(nearestTargetCircle.transform.forward, inSightAngularSpeed);
            }
            else
            {
                nearestTargetCircle.gameObject.SetActive(false);
            }
        }

        public void OnLockOn()
        {
            _runningTween?.Kill();

            lockCircle.transform.position = _lockCircleInitialPosition;
            lockCircle.transform.localScale = _fadedOutLockCircleScale;
            lockCircle.color = _fadedOutLockCircleColor;
            
            lockCircle.gameObject.SetActive(true);
            
            var colorTween = lockCircle.DOColor(_lockCircleInitialColor, circleTweenTime);
            var scaleTween = lockCircle.transform.DOScale(_lockCircleInitialScale, circleTweenTime);

            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Insert(0, colorTween);
            tweenSequence.Insert(0, scaleTween);
            tweenSequence.OnComplete(() => _runningTween = null);

            _runningTween = tweenSequence;
        }

        public void OnUnlock()
        {
            _runningTween?.Kill();

            var colorTween = lockCircle.DOColor(_fadedOutLockCircleColor, circleTweenTime);
            var scaleTween = lockCircle.transform.DOScale(_fadedOutLockCircleScale, circleTweenTime);

            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Insert(0, colorTween);
            tweenSequence.Insert(0, scaleTween);

            tweenSequence.OnComplete(() =>
            {
                lockCircle.gameObject.SetActive(false);
                _runningTween = null;
            });

            _runningTween = tweenSequence;
        }

        public void OnLockBreak()
        {
            _runningTween?.Kill();

            var colorTween = lockCircle.DOColor(_fadedOutLockCircleColor, breakCircleTweenTime);
            var shakeTween = lockCircle.transform.DOShakePosition(breakCircleTweenTime,  strength: breakCircleShakeStrength, vibrato: breakCircleShakeFrequency);

            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Insert(0, colorTween);
            tweenSequence.Insert(0, shakeTween);

            tweenSequence.OnComplete(() =>
            {
                lockCircle.gameObject.SetActive(false);
                _runningTween = null;
            });

            _runningTween = tweenSequence;
        }

        
    }
}