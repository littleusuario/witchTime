using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager
{
    public State_EnemyNormal enemyNormalState;

    public State_EnemyDeath enemyDeathState;

    public IState CurrentState;
    
    public EnemyStateManager(State_EnemyNormal normalState, State_EnemyDeath deathState) 
    {
        enemyNormalState = normalState;
        enemyDeathState = deathState;

        enemyNormalState.Initialize(this);
        enemyDeathState.Initialize(this);

        CurrentState = enemyNormalState;
    }

    public void SwitchState(IState newState) 
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public void UpdateState() 
    {
        CurrentState.UpdateState();
    }
}
