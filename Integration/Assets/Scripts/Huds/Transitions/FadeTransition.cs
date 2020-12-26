using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Huds.Transitions
{
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
}