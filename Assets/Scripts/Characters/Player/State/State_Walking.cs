using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class State_Walking : IState
{
    RandomMovement randomMovement;
    PlayerStateManager stateManager;
    public event Action eventForward;
    public event Action eventBackward;
    private bool moving;

    public State_Walking(RandomMovement randomMovement, PlayerStateManager stateManager) 
    {
        this.randomMovement = randomMovement;
        this.stateManager = stateManager;
    }
    public void EnterState() { }

    public void UpdateState() 
    {
        ProcesarEntrada();
    }

    void ProcesarEntrada()
    {
        moving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));

        bool isUp = Input.GetKey(KeyCode.W);
        bool isDown = Input.GetKey(KeyCode.S);
        bool isLeft = Input.GetKey(KeyCode.A);
        bool isRight = Input.GetKey(KeyCode.D);

        randomMovement.animator.SetBool("up", isUp);
        randomMovement.animator.SetBool("down", isDown);
        randomMovement.animator.SetBool("left", isLeft);
        randomMovement.animator.SetBool("right", isRight);

        if (isUp && isDown)
        {
            isUp = false;
            isDown = false;
        }

        if (isLeft && isRight)
        {
            isLeft = false;
            isRight = false;
        }

        if (isUp && !randomMovement.CollisionHitbox.CollisionBools[0])
        {
            ICommand moveForward = new CommandMoveForward(randomMovement);
            moveForward.Execute();
            eventForward.Invoke();
        }
        if (isDown && !randomMovement.CollisionHitbox.CollisionBools[1])
        {
            ICommand moveBack = new CommandMoveBackward(randomMovement);
            moveBack.Execute();
            eventBackward.Invoke();
        }
        if (isLeft && !randomMovement.CollisionHitbox.CollisionBools[3])
        {
            ICommand moveLeft = new CommandMoveLeft(randomMovement);
            moveLeft.Execute();
        }
        if (isRight && !randomMovement.CollisionHitbox.CollisionBools[2])
        {
            ICommand moveRight = new CommandMoveRight(randomMovement);
            moveRight.Execute();
        }

        if (Input.GetKey(KeyCode.Space) || stateManager.state_Jumping.Jumping)
        {
            stateManager.ChangeState(stateManager.state_Jumping);
        }

        if (Input.GetMouseButtonDown(0))
        {
            stateManager.ChangeState(stateManager.state_Attacking);
        }
    }


    public void MoverJugador(Vector3 direccionMovimiento)
    {
        Vector3 movimiento = direccionMovimiento * randomMovement.velocidad * Time.deltaTime;
        randomMovement.transform.Translate(movimiento, Space.World);
    }

    public void ExitState() { }
}
