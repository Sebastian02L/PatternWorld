using UnityEngine;
namespace ObjectPoolMinigame
{
    public interface IContext
    {
        IState GetState();
        void SetState(IState state);
    }

}
