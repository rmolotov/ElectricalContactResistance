using System;
using ECR.StaticData;
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

        public event Action<StageStaticData> OnSelect; 

        public void Initialize(StageStaticData staticData, Sprite previewSprite, ToggleGroup toggleGroup)
        {
            cardTitle.text = staticData.StageTitle;
            cardDescription.text = staticData.StageDescription;
            cardImage.sprite = previewSprite;
            
            selectToggle.onValueChanged.AddListener(arg =>
                {
                    OnSelect?.Invoke(arg
                        ? staticData 
                        : null
                    );
                }
            );
            selectToggle.group = toggleGroup;
        }
    }
}