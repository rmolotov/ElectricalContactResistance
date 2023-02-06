using DG.Tweening;
using JetBrains.Annotations;
using RSG;
using RSG.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
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
        protected Promise<bool> Promise;

        [Inject]
        private void Construct()
        {
            // haptic service
            // sound service
        }

        private void Awake() => 
            SetInitialAppearance();

        public virtual Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            if (windowTitle && !string.IsNullOrEmpty(titleText))
                windowTitle.text = titleText;

            SetVisible(true);
            
            return Promise = new Promise<bool>();
        }

        private void SetInitialAppearance()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
            if (windowPanel) 
                windowPanel.localScale = Vector3.one * openingInitialScale;
        }

        protected virtual void Close()
        {
            SetVisible(false);
            if (Promise.IsPending())
                Promise.Resolve(UserAccepted);
        }

        protected void PlaySoundEffect()
        {
            // _soundService.Play(clickEffectKey)
        }

        protected void PlayHapticEffect()
        {
            // _hapticService.Play(clickEffectKey)
        }

        private void SetVisible(bool value)
        {
            canvasGroup.blocksRaycasts = value;
            canvasGroup
                .DOFade(value ? 1 : 0, openingDuration)
                .SetEase(Ease.OutQuad);

            if (windowPanel) windowPanel
                .DOScale(Vector3.one * (value ? 1 : 0.5f), openingDuration)
                .SetEase(value ? Ease.OutBounce : Ease.OutQuad);
        }
    }
}