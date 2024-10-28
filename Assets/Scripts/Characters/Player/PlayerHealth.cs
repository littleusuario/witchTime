using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private float life = 3f;
    [SerializeField] private UnityEvent OnDie = new UnityEvent();

    private void Start()
    {
        OnDie.AddListener(GameManager.Instance.LoadNextLevel);
    }
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
