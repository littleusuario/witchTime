using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoveRight : ICommand
{
    RandomMovement randomMovement;

    public CommandMoveRight(RandomMovement randomMovement)
    {
        this.randomMovement = randomMovement;
    }

    public void Execute() 
    {
        randomMovement.stateManager.state_Walking.MoverJugador(Vector3.right);
    }
}
