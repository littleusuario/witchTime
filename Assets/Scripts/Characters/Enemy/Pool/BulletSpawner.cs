using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private BulletPool bulletpool;
    [SerializeField] private Bullet bullet;

    [SerializeField] private GameObject spawn;

    private int totalbullets = 4;
    private float timer = 0f;
    private float rotation;
    public float Rotation => rotation;


    ShootType shoot;
    private enum ShootType
    {
        Diagonal,
        Straight,
    }
    private void Awake()
    {

    }
    private void Update()
    {
        timer += Time.deltaTime;


        if (timer > 5)
        {
            Shooting();
            timer = 0f;
        }
    }

    private void Shooting()
    {
        if (shoot == ShootType.Diagonal)
        {
            float anglestep = 360 / totalbullets;
            float angle = 0;
            for (int i = 0; i < totalbullets; i++)
            {
                float proyectileDirx = spawn.transform.position.x + Mathf.Sin(angle * MathF.PI / 180) * 0.5f;
                float proyectileDirZ = spawn.transform.position.z + Mathf.Cos(angle * MathF.PI / 180) * 0.5f;

                Vector3 proyectileVector = new Vector3(proyectileDirx, 0, proyectileDirZ);
                Vector3 proyecticleMoveDir = (proyectileVector - spawn.transform.position).normalized * 10;

                Bullet tempBullet = bulletpool.BullPool.Get();
                tempBullet.GetComponent<Rigidbody>().velocity = new Vector3(proyecticleMoveDir.x, 0, proyecticleMoveDir.z);
                angle += anglestep;
            }
        }
    }





    private void ChangeDirection()
    {
        switch (shoot)
        {
            case ShootType.Diagonal:
                shoot = ShootType.Straight;
                rotation = 90;
                break;
            case ShootType.Straight:
                shoot = ShootType.Diagonal;
                rotation = 45;
                break;
        }

    }
}






