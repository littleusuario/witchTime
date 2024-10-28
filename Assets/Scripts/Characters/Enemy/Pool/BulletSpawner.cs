using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private BulletPool bulletpool;
    [SerializeField] private GameObject spawn;
    [SerializeField] private FirstEnemy subjecttoObserve;
    [SerializeField] private Intervals intervalsToObserve;
    
    private Ishoot shoot;
    private Straightshoot straightShoot;
    private Diagonalshoot diagonalShoot;
    private void Awake()
    {
        shoot = GetComponent<Ishoot>();
        straightShoot = GetComponent<Straightshoot>();
        diagonalShoot = GetComponent<Diagonalshoot>();
        shoot = diagonalShoot;
      
        if (subjecttoObserve != null)
        {
            subjecttoObserve.Ondie += Ondie;
        }

    }

   
    public void Shooting()
    {
        shoot.shoot(bulletpool, spawn);
        setBulletType();
    }
    private void setBulletType()
    {
      
        if (shoot == diagonalShoot) 
        {
            shoot = straightShoot;
        }
        else if (shoot == straightShoot)
        {
            shoot = diagonalShoot;
        }
    }

    private void Ondie()
    {
        Destroy(this);
    }

}














