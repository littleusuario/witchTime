using UnityEngine;

public class CommandMoveBackward : ICommand
{
    RandomMovement randomMovement;

    public CommandMoveBackward(RandomMovement randomMovement)
    {
        this.randomMovement = randomMovement;
    }

    public void Execute()
    {
        randomMovement.stateManager.state_Walking.MoverJugador(Vector3.back);
    }
}
