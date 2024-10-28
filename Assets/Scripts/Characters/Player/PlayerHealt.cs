using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealt : MonoBehaviour
{
    private float life = 3f;

    public void TakeDamage(int damage)
    {
        life -= damage;
        Debug.Log(life);
    }
}
