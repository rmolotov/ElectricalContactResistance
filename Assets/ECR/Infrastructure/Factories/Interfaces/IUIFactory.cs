using System.Threading.Tasks;
using ECR.Meta.Menu;
using ECR.Meta.Shop;
using UnityEngine;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        Task WarmUp();
        void CleanUp();
        Task CreateUIRoot();
        Task<GameObject> CreateHud();
        Task<MenuController> CreateMainMenu();
        Task<ShopWindow> CreateShop();
    }
}