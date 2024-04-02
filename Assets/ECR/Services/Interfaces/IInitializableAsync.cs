using System.Threading;
using System.Threading.Tasks;

namespace ECR.Services.Interfaces
{
    public interface IInitializableAsync
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}