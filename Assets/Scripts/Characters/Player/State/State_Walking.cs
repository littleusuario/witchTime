using System;
using UnityEngine;
public class State_Walking : IState
{
    private RandomMovement randomMovement;
    private PlayerStateManager stateManager;
    private CollisionController collisionController;
    private bool moving;
    private Vector3 velocidadActual;
    private Vector3 velocidadObjetivo;
    private float aceleracion = 10f;
    private float desaceleracion = 30f;
    private Vector3 lastValidPosition;
    private float inputHorizontal;
    private float inputVertical;
    
    public event Action eventForward;
    public event Action eventBackward;

    public State_Walking(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        this.stateManager = stateManager;
        this.collisionController = randomMovement.GroundCheck;
    }
    public void EnterState()
    {
        velocidadActual = Vector3.zero;
        lastValidPosition = randomMovement.transform.position;
    }
    public void UpdateState()
    {
        ProcesarEntrada();

        Vector3 direccionMovimiento = new Vector3(inputHorizontal, 0, inputVertical);

        if (collisionController.RayHit)
        {
            if (moving)
            {
                lastValidPosition = randomMovement.transform.position;
            }

            if (randomMovement.CanProcessInput())
            {
                MoverJugador(direccionMovimiento);
            }
        }
        else
        {
            randomMovement.transform.position = lastValidPosition;
            velocidadActual = Vector3.zero;
        }

        if (!randomMovement.CanProcessInput())
        {
            return;
        }

        randomMovement.animator.SetFloat("Horizontal", inputHorizontal);
        randomMovement.animator.SetFloat("Vertical", inputVertical);


        if (GameManager.Instance.IsGamePaused || !randomMovement.CanMove())
        {
            return; 
        }
    }

    public void ProcesarEntrada()
    {

        inputHorizontal = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        inputVertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        moving = inputHorizontal != 0 || inputVertical != 0;

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