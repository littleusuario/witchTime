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

    bool blockForward;
    bool blockBack;
    bool blockRight;
    bool blockLeft;

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

    bool CheckRayHit(int rayIndex) 
    {
        return Physics.Raycast(randomMovement.CollisionHitbox.StoredRays[rayIndex], out RaycastHit forwardHit, randomMovement.CollisionHitbox.RayLimit);
    }
    void ProcesarEntrada()
    {
        moving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));
        if (randomMovement.CollisionHitbox.StoredRays.Count > 3) 
        {
            blockForward = CheckRayHit(0);
            blockBack = CheckRayHit(1);
            blockRight = CheckRayHit(2);
            blockLeft = CheckRayHit(3);
        }

        if (Input.GetKey(KeyCode.W) && !blockForward)
        {
            ICommand moveForward = new CommandMoveForward(randomMovement);
            moveForward.Execute();

            eventForward.Invoke();
        }
        if (Input.GetKey(KeyCode.S) && !blockBack)
        {
            ICommand moveBack = new CommandMoveBackward(randomMovement);
            moveBack.Execute();

            eventBackward.Invoke();
        }
        if (Input.GetKey(KeyCode.A) && !blockLeft)
        {
            ICommand moveLeft = new CommandMoveLeft(randomMovement);
            moveLeft.Execute();
        }
        if (Input.GetKey(KeyCode.D) && !blockRight)
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
