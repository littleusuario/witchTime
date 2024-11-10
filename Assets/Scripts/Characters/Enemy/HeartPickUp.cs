using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickUp : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float sphereRad;
    public float timeLapse;

    private Vector3 origin;

    private void Update()
    {
        origin = transform.position;
        timeLapse += Time.deltaTime;
        if (timeLapse > 1)
        {
            Collider[] hitColliders = Physics.OverlapSphere(origin, sphereRad);
            foreach (var hit in hitColliders)
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    PlayerHealth addHealth = hit.gameObject.GetComponent<PlayerHealth>();
                    Debug.Log("Esta colisionando");
                    addHealth.Heal(healAmount);

                    Destroy(this.gameObject);
                }
            }
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereRad);
    }
}
