using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
  public static BulletPool instance;
  public Bullet prefab;
  private ObjectPool<Bullet> bullpool;
    [SerializeField] private GameObject spawn;
   [SerializeField] private BulletSpawner rotation;
 public ObjectPool<Bullet> BullPool { get { return bullpool; } set { bullpool = value; } }

    void Awake()
    {
        bullpool = new ObjectPool<Bullet>(CreateBullet, Ontake, OnReturned, OnDestroyed, false, 10, 100);
    }

    private void OnDestroyed(Bullet Bullet)
    {
        Destroy(Bullet.gameObject);
    }

    private void OnReturned(Bullet Bullet)
    {
        Bullet.gameObject.SetActive(false);
    }

    private void Ontake(Bullet Bullet)
    {
      Bullet.gameObject.SetActive(true);
    }

    private Bullet CreateBullet()
    {
      Bullet tempbull = Instantiate(prefab, spawn.transform.position, Quaternion.Euler(0,rotation.Rotation,0));
    tempbull.gameObject.SetActive(false);
    tempbull.Pool = bullpool;
    return tempbull;

    }
}
