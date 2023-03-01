using DG.Tweening;
using ECR.Services.Economy;
using ECR.UI.Windows;
using RSG;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ECR.Meta.Shop
{
    public class ShopWindow : OneButtonWindow
    {
        private IEconomyService _economyService;

        [Title("Wallet")]
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private Image currencyFill;
        [SerializeField] private float currencyTweensDuration;
        
        public RectTransform itemsCardsContainer;

        [Inject]
        private void Construct(IEconomyService economyService)
        {
            _economyService = economyService;
        }

        public override Promise<bool> InitAndShow<T>(T data, string titleText = "")
        {
            _economyService.OnCurrencyChanged += ChangeCurrencyText;
            _economyService.OnCompletedBuying += BlinkCurrencyText;

            currencyText.text = _economyService.PlayerCurrency.ToString();
            
            return base.InitAndShow(data, titleText);
        }

        protected override void Close()
        {
            _economyService.OnCurrencyChanged -= ChangeCurrencyText;
            _economyService.OnCompletedBuying -= BlinkCurrencyText;
            
            base.Close();
        }

        private void BlinkCurrencyText(bool success)
        {
            if (!success)
                currencyFill
                    .DOFade(1, currencyTweensDuration / 4)
                    .SetLoops(4, LoopType.Yoyo);
        }

        private void ChangeCurrencyText(int value)
        {
            // refactor for <int, int> Rx/Promise
            var current = int.Parse(currencyText.text);
            currencyText
                .DOCounter(current, value, currencyTweensDuration, false)
                .SetEase(Ease.OutQuad);
        }
    }
}