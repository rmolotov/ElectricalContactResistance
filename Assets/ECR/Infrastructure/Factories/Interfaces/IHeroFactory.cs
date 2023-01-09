using System.Threading.Tasks;
using UnityEngine;

namespace ECR.Infrastructure.Factories.Interfaces
{
    public interface IHeroFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<GameObject> Create(Vector3 at);
    }
}