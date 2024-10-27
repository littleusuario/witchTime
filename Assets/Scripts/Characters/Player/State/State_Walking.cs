using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class State_Walking : IState
{
    RandomMovement randomMovement;
    PlayerStateManager stateManager;
    public event Action eventForward;
    public event Action eventBackward;
    private bool moving;

    private Vector3 velocidadActual;
    private Vector3 velocidadObjetivo;
    private float aceleracion = 10f;
    private float desaceleracion = 30f;

    public State_Walking(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        this.stateManager = stateManager;
    }

    public void EnterState()
    {
        velocidadActual = Vector3.zero;
    }

    public void UpdateState()
    {
        ProcesarEntrada();

        Vector3 direccionMovimiento = Vector3.zero;

        if (moving)
        {
            if (Input.GetKey(KeyCode.W))
                direccionMovimiento += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
                direccionMovimiento += Vector3.back;
            if (Input.GetKey(KeyCode.A))
                direccionMovimiento += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                direccionMovimiento += Vector3.right;
        }

        MoverJugador(direccionMovimiento);
    }

    public void ProcesarEntrada()
    {
        moving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        randomMovement.animator.SetBool("up", Input.GetKey(KeyCode.W));
        randomMovement.animator.SetBool("down", Input.GetKey(KeyCode.S));
        randomMovement.animator.SetBool("left", Input.GetKey(KeyCode.A));
        randomMovement.animator.SetBool("right", Input.GetKey(KeyCode.D));

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
        if (direccionMovimiento != Vector3.zero)
        {
            velocidadObjetivo = direccionMovimiento.normalized * randomMovement.velocidad;
        }
        else
        {
            velocidadObjetivo = Vector3.zero;
        }

        if (moving)
        {
            float currentSpeed = velocidadActual.magnitude;
            float targetSpeed = velocidadObjetivo.magnitude;

            if (currentSpeed > targetSpeed)
            {
                velocidadActual = Vector3.MoveTowards(velocidadActual, velocidadObjetivo, (desaceleracion * 5) * Time.deltaTime);
            }
            else
            {
                velocidadActual = Vector3.MoveTowards(velocidadActual, velocidadObjetivo, (aceleracion * 2) * Time.deltaTime);
            }
        }
        else
        {
            velocidadActual = Vector3.MoveTowards(velocidadActual, Vector3.zero, desaceleracion * Time.deltaTime);
        }

        Vector3 movimiento = velocidadActual * Time.deltaTime;
        randomMovement.transform.Translate(movimiento, Space.World);
    }


    public void ExitState() { }
}
