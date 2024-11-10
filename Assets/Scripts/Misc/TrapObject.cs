using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapObject : MonoBehaviour
{
    protected GameObject player;

    protected void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public abstract void TrapActivate();
}
