using System;
using ECR.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECR.Meta.Shop
{
    public class ShopItemCard: MonoBehaviour
    {
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI itemTitle;
        [SerializeField] private TextMeshProUGUI itemDescription;
        
        [SerializeField] private TextMeshProUGUI obtainedCountText;
        [SerializeField] private TextMeshProUGUI availableCountText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Button buyButton;
        
        public event Action<string> OnBuyClick;

        private InventoryItemStaticData _staticData;

        public void Initialize(InventoryItemStaticData staticData, Sprite previewSprite, int obtainedCount, int timerSeconds = 0)
        {
            _staticData = staticData;
            
            cardImage.sprite = previewSprite;
            itemTitle.text = _staticData.Title;
            itemDescription.text = _staticData.Description;
            priceText.text = _staticData.Price.ToString();

            UpdateObtainedCount(obtainedCount);
            
            //timerText.text = $"{timerSeconds / 60}:{timerSeconds % 60}";
            // TODO: disable buyButton when timer (limited deal) expired

            buyButton.onClick.AddListener(() =>
                OnBuyClick?.Invoke(staticData.ItemId));
        }

        public void UpdateObtainedCount(int obtainedCount)
        {
            var available = _staticData.MaxCount - obtainedCount;
            
            obtainedCountText.text = obtainedCount.ToString();
            availableCountText.text = available.ToString();
            buyButton.interactable = available > 0;
        }
    }
}