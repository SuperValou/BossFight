using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Scripts.Huds.Transitions
{
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