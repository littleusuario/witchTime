using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealt : MonoBehaviour
{
    private float life = 3f;
    [SerializeField] private UnityEvent OnDie;

    public void TakeDamage(int damage)
    {
        life -= damage;
        Debug.Log(life);
        if (life == 0)
        {
            OnDie.Invoke();
        }    
    }
}
