using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    ObjectPool<Bullet> pool;
    BulletPool bulletPool;
    [SerializeField] private float sphereRad;
    private Vector3 origin;
    private Vector3 direction;
    
    public LayerMask mask;
    public float maxdist;
   public ObjectPool<Bullet> Pool { get { return pool; } set { pool = value; } }


    private void Update()
    {
        origin = transform.position;
        direction = transform.position;
        RaycastHit hit;

        if (Physics.SphereCast(origin,sphereRad,direction, out hit,maxdist,mask, QueryTriggerInteraction.UseGlobal))
        {
            if ( hit.collider.CompareTag("Wall"))
            {
                Pool.Release(this);
                Debug.Log("salio");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,sphereRad);
    }
}





