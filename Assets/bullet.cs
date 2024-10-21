using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    ObjectPool<Bullet> pool;
   [SerializeField] private Rigidbody rb;
    BulletPool bulletPool;
   public ObjectPool<Bullet> Pool { get { return pool; } set { pool = value; } }


  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
   
            pool.Release(this);

           
        }
    }
}





