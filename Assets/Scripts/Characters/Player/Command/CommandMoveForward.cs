using UnityEngine;

public class CommandMoveForward : ICommand
{
    RandomMovement randomMovement;

    public CommandMoveForward(RandomMovement randomMovement)
    {
        this.randomMovement = randomMovement;
    }

    public void Execute()
    {
        randomMovement.stateManager.state_Walking.MoverJugador(Vector3.forward);
    }
}
