using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image.FillMethod;

namespace ECR.Gameplay.UI
{
    public class HealthBar : MonoBehaviour
    {
        [InfoBox(
            "Please set appropriate transition duration to \"Death\" state in animator based on \"Hit\" animation clip"
            )]
        [SerializeField] private float damageTweensDuration;
        [SerializeField] private bool alwaysVisible;
        [SerializeField] private Image barImage;

        private Sequence _sequence;

        private void OnDestroy() => 
            _sequence.Kill();

        public void SetValue(int current, int max)
        {
            var normalizedHp = 1f * current / max;

            _sequence.Complete();
            _sequence = DOTween.Sequence()
                .AppendCallback(() => gameObject
                    .SetActive(true))
                .Append(barImage
                    .DOFillAmount(normalizedHp, damageTweensDuration))
                .Join(barImage.rectTransform
                    .DOLocalRotate(SelectRotation(normalizedHp, barImage), damageTweensDuration))
                .AppendCallback(() => gameObject
                    .SetActive(alwaysVisible == true))
                .Play();
        }

        private static Vector3 SelectRotation(float normalizedValue, Image image) =>
            image.fillMethod switch
            {
                Horizontal or Vertical => Vector3.zero,
                Radial360              => new Vector3(0, 0, 45 + (1 - normalizedValue) * 180),
                Radial180              => new Vector3(0, 0, (1 - normalizedValue) * 90),
                _                      => throw new ArgumentOutOfRangeException()
            };
    }
}