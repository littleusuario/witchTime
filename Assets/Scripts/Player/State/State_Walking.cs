using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class State_Walking : IState
{
    RandomMovement randomMovement;
    public event Action eventForward;
    public event Action eventBackward;
    private bool moving;

    public State_Walking(RandomMovement randomMovement) 
    {
        this.randomMovement = randomMovement;
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

        if (Input.GetKey(KeyCode.W))
        {
            ICommand moveForward = new CommandMoveForward(randomMovement);
            moveForward.Execute();
            var particleSystemMain = randomMovement.SlashParticle;

            particleSystemMain.transform.eulerAngles = randomMovement.ParticleRotations[0];
            eventForward.Invoke();
        }
        if (Input.GetKey(KeyCode.S))
        {
            ICommand moveBack = new CommandMoveBackward(randomMovement);
            moveBack.Execute();

            var particleSystemMain = randomMovement.SlashParticle;

            particleSystemMain.transform.eulerAngles = randomMovement.ParticleRotations[1];
            eventBackward.Invoke();
        }
        if (Input.GetKey(KeyCode.A))
        {
            ICommand moveLeft = new CommandMoveLeft(randomMovement);
            moveLeft.Execute();

            var particleSystemMain = randomMovement.SlashParticle;

            particleSystemMain.transform.eulerAngles = randomMovement.ParticleRotations[2];
        }
        if (Input.GetKey(KeyCode.D))
        {
            ICommand moveRight = new CommandMoveRight(randomMovement);
            moveRight.Execute();

            var particleSystemMain = randomMovement.SlashParticle;

            particleSystemMain.transform.eulerAngles = randomMovement.ParticleRotations[3];
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
