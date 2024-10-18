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
        AnimacionMovimiento();
    }
    void ProcesarEntrada()
    {
        moving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));

        if (Input.GetKey(KeyCode.W) && !randomMovement.CollisionHitbox.CollisionBools[0])
        {
            ICommand moveForward = new CommandMoveForward(randomMovement);
            moveForward.Execute();

            eventForward.Invoke();
        }
        if (Input.GetKey(KeyCode.S) && !randomMovement.CollisionHitbox.CollisionBools[1])
        {
            ICommand moveBack = new CommandMoveBackward(randomMovement);
            moveBack.Execute();

            eventBackward.Invoke();
        }
        if (Input.GetKey(KeyCode.A) && !randomMovement.CollisionHitbox.CollisionBools[3])
        {
            ICommand moveLeft = new CommandMoveLeft(randomMovement);
            moveLeft.Execute();
        }
        if (Input.GetKey(KeyCode.D) && !randomMovement.CollisionHitbox.CollisionBools[2])
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

    private void AnimacionMovimiento()
    {
        if (randomMovement.animator != null)
        {
            randomMovement.animator.SetBool("walking", moving);
        }
    }
    public void ExitState() { }
}
