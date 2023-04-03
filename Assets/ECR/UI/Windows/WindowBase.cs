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
            // sound service
        }

        private void Awake() => 
            SetInitialAppearance();

        public virtual Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            var text = data as string;
            
            if (windowTitle && !string.IsNullOrEmpty(titleText))
                windowTitle.text = titleText;

            if (windowText && !string.IsNullOrEmpty(text))
                windowText.text = text;

            SetVisible(true);
            
            return Promise = new Promise<bool>();
        }

        protected virtual void Close() =>
            SetVisible(false)
                .Then(() => 
                    Promise.ResolveIfPending(UserAccepted));


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

        private Promise SetVisible(bool value)
        {
            if (canvasGroup) canvasGroup.blocksRaycasts = value;
            
            var animationPromise = new Promise();
            
            DOTween.Sequence()
                .Append(canvasGroup?
                    .DOFade(value ? 1 : 0, openingDuration)
                    .SetEase(Ease.OutQuad))
                .Join(windowPanel?
                    .DOScale(Vector3.one * (value ? 1 : 0.5f), openingDuration)
                    .SetEase(value ? Ease.OutBounce : Ease.OutQuad))
                .Play()
                .onComplete += () => 
                    animationPromise.ResolveIfPending();

            return animationPromise;
        }
    }
}