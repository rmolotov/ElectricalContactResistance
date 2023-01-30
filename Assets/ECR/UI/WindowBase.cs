using System;
using DG.Tweening;
using JetBrains.Annotations;
using RSG;
using RSG.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace ECR.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowBase : MonoBehaviour
    {
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
        protected virtual void Construct()
        {

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

        }

        protected void PlayHapticEffect()
        {

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