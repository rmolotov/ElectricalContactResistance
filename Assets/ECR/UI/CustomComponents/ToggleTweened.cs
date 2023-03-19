using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.UI.CustomComponents
{
    [AddComponentMenu("UI/Toggle Tweened")]
    [RequireComponent(typeof(Toggle))]
    public class ToggleTweened : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private TextMeshProUGUI onText, offText;
        [SerializeField] private Image handle;
        [SerializeField] private float duration;

        private static readonly Vector2 OffMin = new(0.0f, 0);
        private static readonly Vector2 OffMax = new(0.5f, 1);
        private static readonly Vector2 OnMin  = new(0.5f, 0);
        private static readonly Vector2 OnMax  = new(1.0f, 1);
        private const float HandleFade         =     0.5f;

        private void OnEnable() =>
            toggle.onValueChanged.AddListener(Switch);

        private void OnDisable() =>
            toggle.onValueChanged.RemoveListener(Switch);

        
        private void Switch(bool value)
        {
            TweenText(onText, value);
            TweenText(offText, !value);
            TweenHandle(value);
        }

        private void TweenText(TextMeshProUGUI text, bool value) =>
            text
                .DOFade(value ? 1 : 0, duration)
                .SetEase(Ease.OutQuad);

        private void TweenHandle(bool value) =>
            DOTween.Sequence()
                .Append(handle.rectTransform
                    .DOAnchorMin(value ? OnMin : OffMin, duration)
                    .SetEase(value ? Ease.InQuad : Ease.OutQuad))
                .Join(handle.rectTransform
                    .DOAnchorMax(value ? OnMax : OffMax, duration)
                    .SetEase(value ? Ease.OutQuad : Ease.InQuad))
                .Join(handle
                    .DOFade(HandleFade, duration / 2)
                    .SetLoops(2, LoopType.Yoyo))
                .Play();
    }
}