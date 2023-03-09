using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.Gameplay.UI
{
    public class HealthBar : MonoBehaviour
    {
        [InfoBox(
            "Please set appropriate transition duration to \"Death\" state in animator based on \"Hit\" animation clip",
            InfoMessageType.Info)]
        [SerializeField] private float damageTweensDuration;
        [SerializeField] private Image barImage;

        private Sequence sequence;

        private void OnDestroy() => 
            sequence.Kill();

        public void SetValue(int current, int max)
        {
            var normalizedHp = 1f * current / max;

            sequence.Complete();
            sequence = DOTween.Sequence()
                .AppendCallback(() => gameObject
                    .SetActive(true))
                .Append(barImage
                    .DOFillAmount(normalizedHp, damageTweensDuration))
                .Join(barImage.rectTransform
                    .DOLocalRotate(new Vector3(0, 0, 45 + (1 - normalizedHp) * 180), damageTweensDuration))
                .AppendCallback(() => gameObject
                    .SetActive(false))
                .Play();
        }
    }
}