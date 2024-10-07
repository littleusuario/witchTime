using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Variables visuales")]
    [SerializeField] Quaternion rotacionObjetivo;
    [SerializeField] Transform objetoAGirar;
    RandomMovement RandomMovement;
    private void Awake()
    {
        RandomMovement = GetComponentInParent<RandomMovement>();
        RandomMovement.stateManager.state_Walking.eventForward += FaceForward;
        RandomMovement.stateManager.state_Walking.eventBackward += FaceBackward;
    }

    void FaceForward() 
    {
        rotacionObjetivo = Quaternion.Euler(0, 180, 0);
    }

    void FaceBackward() 
    {
        rotacionObjetivo = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        GirarObjeto();
    }
    void GirarObjeto()
    {
        if (objetoAGirar != null)
        {
            float velocidadGiro = 10f;
            objetoAGirar.rotation = Quaternion.Lerp(objetoAGirar.rotation, rotacionObjetivo, Time.deltaTime * velocidadGiro);
        }
    }
}
