using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
  


    [Header("Bullet Attributes")]
    public GameObject bullet;

    [SerializeField] GameObject spawn;
    private float rotation;
    private const float radius = 0.5f;  
    private int totalbullets = 2;
    public float Rotation => rotation;
    enum Direction
    {
        Diagonal,
        Straight,
    }
    private Direction direction;
    [SerializeField] private float firingRate = 2f;
    
    private float timer = 0f;
    // Start is called before the first frame update
  


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Shoot();
    }

    private void Shoot()
    {


        if (timer > firingRate)
        {
            if (direction == Direction.Diagonal)
            {
                for (int i = 0; i < totalbullets; i++)
                {
                    float proyectileDirXposition = spawn.transform.position.x + MathF.Sin(Rotation * MathF.PI) / 180 * radius;
                    float proyectileDirZposition = spawn.transform.position.z + MathF.Tan(rotation * MathF.PI) / 180 * radius;
                    Vector3 bulletvector = new Vector3(proyectileDirXposition, 0, proyectileDirZposition);
                    Vector3 BulletDir = (bulletvector - spawn.transform.position).normalized * 10;

                    GameObject tempBullet = Instantiate(bullet, spawn.transform.position, quaternion.Euler(rotation), spawn.transform);
                    tempBullet.GetComponent<Rigidbody>().velocity = new Vector3(BulletDir.x, 0, BulletDir.z);
                    rotation += 90;
                }
              
            }

            if (direction == Direction.Straight)
            {
                for (int i = 0; i < totalbullets; i++)
                {
                    float proyectileDirXposition = spawn.transform.position.x + MathF.Sin(Rotation * MathF.PI) / 180 * radius;
                    float proyectileDirZposition = spawn.transform.position.z + MathF.Tan(rotation * MathF.PI) / 180 * radius;
                    Vector3 bulletvector = new Vector3(proyectileDirXposition, 0, proyectileDirZposition);
                    Vector3 BulletDir = (bulletvector - spawn.transform.position).normalized * 10;

                    GameObject tempBullet = Instantiate(bullet, spawn.transform.position, quaternion.Euler(rotation), spawn.transform);
                    tempBullet.GetComponent<Rigidbody>().velocity = (new Vector3(BulletDir.x, 0, BulletDir.z));
                    rotation += 90;
                }
              
            }
            changedir();
            timer = 0f;
        }
       
    }

    void changedir()
    {
        switch (direction)
        {
            case Direction.Diagonal:
                {
                    direction = Direction.Straight;
                    rotation = 90;
                    break;
                }
            case Direction.Straight:
                {
                    direction = Direction.Diagonal;
                    rotation = 45;
                    break;
                }
        }

    }

}


