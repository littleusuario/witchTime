using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diagonalshoot : MonoBehaviour, Ishoot
{
    private int totalbullets = 4;

    public void shoot(BulletPool bullet, GameObject spawn)
    {
        float diagonalAngleStep = 360 / totalbullets;
        float diagonalAngle = 45;
        for (int i = 0; i < totalbullets; i++)
        {
            float proyectileDirx2 = spawn.transform.position.x + Mathf.Sin(diagonalAngle * MathF.PI / 180) * 0.5f;
            float proyectileDirZ2 = spawn.transform.position.z + Mathf.Cos(diagonalAngle * MathF.PI / 180) * 0.5f;

            Vector3 proyectileVector2 = new Vector3(proyectileDirx2, 0, proyectileDirZ2);
            Vector3 proyecticleMoveDir2 = (proyectileVector2 - spawn.transform.position).normalized * 10;

            Bullet tempBullet = bullet.BullPool.Get();
            tempBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            tempBullet.transform.position = proyectileVector2;
            tempBullet.GetComponent<Rigidbody>().velocity = new Vector3(proyecticleMoveDir2.x, 0, proyecticleMoveDir2.z);
            diagonalAngle += diagonalAngleStep;
        }
        
    }
}
