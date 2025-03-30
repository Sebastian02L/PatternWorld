using UnityEngine;
namespace ObserverMinigame
{
    public interface IContext
    {
        IState GetState();
        void SetState(IState state);
        void EvaluatePostMoveTransition();
        void EvaluateInterruptionTransition();
    }

}
