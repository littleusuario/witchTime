using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    public IState currentState;
    public State_Walking state_Walking;

    public StateManager(RandomMovement randomMovement) 
    {
        state_Walking = new State_Walking(randomMovement);

        currentState = state_Walking;
    }

    public void changeState(IState newState) 
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
