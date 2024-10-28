using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int HealthPoints;
    public abstract event Action Ondie;
    public abstract void RunBehaviour();
}
