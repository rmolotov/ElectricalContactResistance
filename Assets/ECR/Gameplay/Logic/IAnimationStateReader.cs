namespace ECR.Gameplay.Logic
{
    public interface IAnimationStateReader
    {
        AnimatorState State { get; }
        
        void OnEnter(int stateHash);
        void OnExit(int stateHash);
    }
}