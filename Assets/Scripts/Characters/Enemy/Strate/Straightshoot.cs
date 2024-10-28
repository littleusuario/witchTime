using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straightshoot : MonoBehaviour, Ishoot
{
    private int totalbullets = 4;
    void Ishoot.shoot(BulletPool bullet, GameObject spawn)
    {
        float anglestep = 360 / totalbullets;
        float angle = 0;
        for (int i = 0; i < totalbullets; i++)
        {
            float proyectileDirx = spawn.transform.position.x + Mathf.Sin(angle * MathF.PI / 180) * 0.5f;
            float proyectileDirZ = spawn.transform.position.z + Mathf.Cos(angle * MathF.PI / 180) * 0.5f;

            Vector3 proyectileVector = new Vector3(proyectileDirx, 0, proyectileDirZ);
            Vector3 proyecticleMoveDir = (proyectileVector - spawn.transform.position).normalized * 10;


            Bullet tempBullet = bullet.BullPool.Get();
            tempBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            tempBullet.transform.position = proyectileVector;
            tempBullet.GetComponent<Rigidbody>().velocity = new Vector3(proyecticleMoveDir.x, 0, proyecticleMoveDir.z);
            angle += anglestep;
        }

    }
}
