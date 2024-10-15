using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    public IState currentState;
    public State_Walking state_Walking;
    public State_Jump state_Jumping;
    public State_Attack state_Attacking;
    public StateManager(RandomMovement randomMovement) 
    {
        state_Walking = new State_Walking(randomMovement, this);
        state_Jumping = new State_Jump(randomMovement, this);
        state_Attacking = new State_Attack(randomMovement, this);

        currentState = state_Walking;
    }

    public void ChangeState(IState newState) 
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void Update() 
    {
        if (currentState != null) 
        {
            currentState.UpdateState();
        }
    }
}
