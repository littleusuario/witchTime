using System;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Variables de movimiento")]
    public float velocidad = 2f;

    public Animator animator;
    private Camera mainCamera;

    public StateManager stateManager;
    void Awake()
    {
        stateManager = new StateManager(this);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    }

    void Update()
    {
        stateManager.Update();
        //ProcesarEntrada();
        //AnimacionMovimiento();
    }

    //void ProcesarEntrada()
    //{
    //    moving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));

    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        ICommand moveForward = new CommandMoveForward(this);
    //        moveForward.Execute();
    //        eventForward.Invoke();
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        ICommand moveBack = new CommandMoveBackward(this);
    //        moveBack.Execute();
    //        eventBackward.Invoke();
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        ICommand moveLeft = new CommandMoveLeft(this);
    //        moveLeft.Execute();
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        ICommand moveRight = new CommandMoveRight(this);
    //        moveRight.Execute();
    //    }
    //}

    //public void MoverJugador(Vector3 direccionMovimiento)
    //{
    //    Vector3 movimiento = direccionMovimiento * velocidad * Time.deltaTime;
    //    transform.Translate(movimiento, Space.World);
    //}

    //private void AnimacionMovimiento()
    //{
    //    if (animator != null)
    //    {
    //        animator.SetBool("walking", moving);
    //    }
    //}
}
