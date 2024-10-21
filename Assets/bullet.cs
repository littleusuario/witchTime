using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    ObjectPool<Bullet> pool;
   public ObjectPool<Bullet> Pool { get { return pool; } set { pool = value; } }
}





