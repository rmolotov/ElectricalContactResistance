using System.Threading.Tasks;
using UnityEngine;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;
using Zenject;

namespace ECR.UI.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
        private const string ClickEffectKey = "SFX_buttonClick";
        
        [Title("Appearance settings")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] [CanBeNull] private RectTransform windowPanel;
        [SerializeField] [Range(0.1f, 10f)] private float openingDuration;
        [SerializeField] [Range(0f, 1f)] private float openingInitialScale;

        [Title("Text elements")]
        [SerializeField] protected TextMeshProUGUI windowTitle;
        [SerializeField] protected TextMeshProUGUI windowText;
        
        protected bool UserAccepted;
        protected TaskCompletionSource<bool> Promise;

        [Inject]
        private void Construct()
        {
            // sound service
        }

        private void Awake() => 
            SetInitialAppearance();

        public virtual TaskCompletionSource<bool> InitAndShow<T>(T data, string titleText = "")
        {
            var text = data as string;
            
            if (windowTitle && !string.IsNullOrEmpty(titleText))
                windowTitle.text = titleText;

            if (windowText && !string.IsNullOrEmpty(text))
                windowText.text = text;

            SetVisible(true);
            
            return Promise = new TaskCompletionSource<bool>();
        }

        protected virtual void Close() =>
            SetVisible(false).onComplete += () =>
                Promise.SetResult(UserAccepted);


        protected void PlaySoundEffect()
        {
            // _soundService.Play(clickEffectKey)
        }

        protected void PlayHapticEffect()
        {
            // _hapticService.Play(clickEffectKey)
        }

        private void SetInitialAppearance()
        {
            if (canvasGroup) (canvasGroup.blocksRaycasts, canvasGroup.alpha) = (false, 0);
            if (windowPanel) windowPanel.localScale = Vector3.one * openingInitialScale;
        }

        private Sequence SetVisible(bool value) =>
            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    if (canvasGroup) canvasGroup.blocksRaycasts = value;
                })
                .Append(canvasGroup?
                    .DOFade(value ? 1 : 0, openingDuration)
                    .SetEase(Ease.OutQuad))
                .Join(windowPanel?
                    .DOScale(Vector3.one * (value ? 1 : 0.5f), openingDuration)
                    .SetEase(value ? Ease.OutBounce : Ease.OutQuad))
                .Play();
    }
}