using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using Zenject;
using CustomExtensions.Rx;
using ECR.Services.Economy;
using ECR.UI.Windows;

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
        
        private readonly CompositeDisposable _disposables = new();

        [Inject]
        private void Construct(IEconomyService economyService) => 
            _economyService = economyService;

        public override TaskCompletionSource<bool> InitAndShow<T>(T data, string titleText = "")
        {
            Observable
                .FromEvent<bool>(
                    x => _economyService.OnCompletedBuying += x,
                    x => _economyService.OnCompletedBuying -= x)
                .Where(success => success == false)
                .Subscribe(_ => currencyFill
                    .DOFade(1, currencyTweensDuration / 4)
                    .SetLoops(4, LoopType.Yoyo))
                .AddTo(_disposables);

            _economyService.PlayerCurrency
                .CombineWithPrevious((prev, next) => (prev, next))
                .Subscribe(tuple => currencyText
                    .DOCounter(tuple.prev, tuple.next, currencyTweensDuration, false)
                    .SetEase(Ease.OutQuad));

            return base.InitAndShow(data, titleText);
        }

        protected override void Close()
        {
            _disposables.Clear();

            currencyFill.DOComplete();
            currencyText.DOComplete();
            
            base.Close();
        }
    }
}