using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Variables de movimiento")]
    public float velocidad = 2f;
    private Vector3 direccionMovimiento = Vector3.zero;

    [Header("Variables visuales")]
    public Transform objetoAGirar;
    private Quaternion rotacionObjetivo;

    public Animator animator;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    }

    void Update()
    {
        ProcesarEntrada();

        if (direccionMovimiento != Vector3.zero)
        {
            MoverJugador();
            ActualizarEstado();
            GirarObjeto();
        }
        else
        {
            DetenerMovimiento();
        }
    }

    void ProcesarEntrada()
    {
        direccionMovimiento = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direccionMovimiento += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direccionMovimiento += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direccionMovimiento += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direccionMovimiento += Vector3.right;
        }

        direccionMovimiento.Normalize();
    }

    void MoverJugador()
    {
        Vector3 movimiento = direccionMovimiento * velocidad * Time.deltaTime;
        transform.Translate(movimiento, Space.World);

        if (animator != null)
        {
            animator.SetBool("walking", true);
        }
    }

    void DetenerMovimiento()
    {
        if (animator != null)
        {
            animator.SetBool("walking", false);
        }
    }

    void ActualizarEstado()
    {
        if (direccionMovimiento.z > 0)
        {
            rotacionObjetivo = Quaternion.Euler(0, 180, 0);
        }
        else if (direccionMovimiento.z < 0)
        {
            rotacionObjetivo = Quaternion.Euler(0, 0, 0);
        }
    }

    void GirarObjeto()
    {
        if (objetoAGirar != null)
        {
            if (direccionMovimiento.z != 0)
            {
                float velocidadGiro = 10f;
                objetoAGirar.rotation = Quaternion.Lerp(objetoAGirar.rotation, rotacionObjetivo, Time.deltaTime * velocidadGiro);
            }
        }
    }
}
