using UnityEngine;
public interface IContext
{
    IState GetState();
    void SetState(IState state);
}
