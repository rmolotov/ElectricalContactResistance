using System.Threading.Tasks;
using ECR.Meta.Hud;
using ECR.Meta.Menu;
using ECR.Meta.Shop;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        Task WarmUp();
        void CleanUp();
        Task CreateUIRoot();
        Task<HudController> CreateHud();
        Task<MenuController> CreateMainMenu();
        Task<ShopWindow> CreateShop();
    }
}