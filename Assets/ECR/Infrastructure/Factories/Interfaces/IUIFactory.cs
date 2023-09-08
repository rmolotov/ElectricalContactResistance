using System.Threading.Tasks;
using ECR.Meta.HUD;
using ECR.Meta.Menu;
using ECR.Meta.Shop;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        Task WarmUp();
        void CleanUp();
        Task CreateUIRoot();
        Task<HUDController> CreateHud();
        Task<MenuController> CreateMainMenu();
        Task<ShopWindow> CreateShop();
    }
}