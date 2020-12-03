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

        void Update()
        {
            if (lockOnManager.HasTargetLocked)
            {
                lockCircle.transform.Rotate(lockCircle.transform.forward, lockedAngularSpeed);
            }

            if (lockOnManager.HasAnyTargetInSight)
            {
                Vector2 viewportPosition = lockOnManager.NearestTargetViewportPosition;
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

    internal class FadeTransition
    {
        private readonly float _fadeDuration;
        private readonly float _scaleMultiplier;

        private Vector3 _initialPosition;
        private Vector3 _initialScale;
        private Color _initialColor;

        protected Image Image { get; }

        protected Color OutColor { get; private set; }
        protected Vector3 OutScale { get; private set; }

        protected Tween RunningTween { get; set; }

        public FadeTransition(Image image, float scaleMultiplier, float fadeDuration)
        {
            Image = image;

            _scaleMultiplier = scaleMultiplier;
            _fadeDuration = fadeDuration;
        }

        public void Initialize()
        {
            _initialPosition = Image.transform.position;
            _initialScale = Image.transform.localScale;
            _initialColor = Image.color;

            OutColor = new Color(_initialColor.r, _initialColor.g, _initialColor.b, a: 0);
            OutScale = Image.transform.localScale * _scaleMultiplier;

            Image.gameObject.SetActive(false);
        }

        public void FadeIn()
        {
            RunningTween?.Kill();

            Image.transform.position = _initialPosition;
            Image.transform.localScale = OutScale;
            Image.color = OutColor;

            Image.gameObject.SetActive(true);

            var colorTween = Image.DOColor(_initialColor, _fadeDuration);
            var scaleTween = Image.transform.DOScale(_initialScale, _fadeDuration);

            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Insert(0, colorTween);
            tweenSequence.Insert(0, scaleTween);
            tweenSequence.OnComplete(() => RunningTween = null);

            RunningTween = tweenSequence;
        }

        public void FadeOut()
        {
            RunningTween?.Kill();

            var colorTween = Image.DOColor(OutColor, _fadeDuration);
            var scaleTween = Image.transform.DOScale(OutScale, _fadeDuration);

            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Insert(0, colorTween);
            tweenSequence.Insert(0, scaleTween);

            tweenSequence.OnComplete(() =>
            {
                Image.gameObject.SetActive(false);
                RunningTween = null;
            });

            RunningTween = tweenSequence;
        }
    }

    internal class ShakeOutFadeTransition : FadeTransition
    {
        private readonly float _shakeTime;
        private readonly float _shakeStrength;
        private readonly int _shakeFrequency;

        public ShakeOutFadeTransition(Image image, float scaleMultiplier, float fadeDuration, float shakeTime, float shakeStrength, int shakeFrequency) 
            : base(image, scaleMultiplier, fadeDuration)
        {
            _shakeTime = shakeTime;
            _shakeStrength = shakeStrength;
            _shakeFrequency = shakeFrequency;
        }

        public void ShakeOut()
        {
            RunningTween?.Kill();

            var colorTween = Image.DOColor(OutColor, _shakeTime);
            var shakeTween = Image.transform.DOShakePosition(_shakeTime, strength: _shakeStrength, vibrato: _shakeFrequency);

            Sequence tweenSequence = DOTween.Sequence();
            tweenSequence.Insert(0, colorTween);
            tweenSequence.Insert(0, shakeTween);

            tweenSequence.OnComplete(() =>
            {
                Image.gameObject.SetActive(false);
                RunningTween = null;
            });

            RunningTween = tweenSequence;
        }
    }
}