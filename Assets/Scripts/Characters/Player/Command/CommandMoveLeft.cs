using UnityEngine;

public class CommandMoveLeft : ICommand
{
    RandomMovement randomMovement;

    public CommandMoveLeft(RandomMovement randomMovement)
    {
        this.randomMovement = randomMovement;
    }

    public void Execute()
    {
        randomMovement.stateManager.state_Walking.MoverJugador(Vector3.left);
    }
}
