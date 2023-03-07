using UniRx;

namespace ECR.Gameplay.Logic
{
    public interface IHealth
    {
        IntReactiveProperty CurrentHP { get; set; }
        int MaxHP { get; set; }

        void TakeDamage(int damage);
    }
}