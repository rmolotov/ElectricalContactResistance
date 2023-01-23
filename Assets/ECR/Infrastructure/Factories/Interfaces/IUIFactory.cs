using System.Threading.Tasks;
using ECR.Meta.Menu;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        Task WarmUp();
        void CleanUp();
        Task CreateUIRoot();
        Task<MenuController> CreateMainMenu();
    }
}