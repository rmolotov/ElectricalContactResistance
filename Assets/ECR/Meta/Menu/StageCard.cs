using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.Meta.Menu
{
    public class StageCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cardTitle;
        [SerializeField] private TextMeshProUGUI cardDescription;
        [SerializeField] private Toggle selectToggle;
        [SerializeField] private Image cardImage;

        public event Action<string> OnSelect; 

        public void Initialize(string stageKey, string title, string description, Sprite previewSprite, ToggleGroup toggleGroup)
        {
            cardTitle.text = title;
            cardDescription.text = description;
            cardImage.sprite = previewSprite;
            
            selectToggle.onValueChanged.AddListener(arg =>
                {
                    if (arg) OnSelect?.Invoke(stageKey);
                }
            );
            selectToggle.group = toggleGroup;
        }
    }
}