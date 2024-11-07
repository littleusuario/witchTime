public abstract class State_EnemyDeath : IState
{
    public EnemyStateManager StateManager;
    public void Initialize(EnemyStateManager stateManager)
    {
        StateManager = stateManager;
    }
    public virtual void EnterState() { }
    public virtual void UpdateState()
    {

    }

    public virtual void ExitState() { }
}
