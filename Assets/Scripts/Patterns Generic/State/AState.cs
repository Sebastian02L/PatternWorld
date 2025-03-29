using UnityEngine;

public abstract class AState : IState
{
    protected IContext context;

    public AState(IContext context)
    {
        this.context = context;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void FixedUpdate();

    public abstract void Update();
}
