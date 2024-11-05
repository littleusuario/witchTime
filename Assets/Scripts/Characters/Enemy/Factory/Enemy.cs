using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int HealthPoints;
    public int AttackRadius = 5;
    public abstract event Action Ondie;
    public ParticleSystem DamageParticleSystem;
    public abstract void RunBehaviour();
    public abstract void DamagaZone();
    public abstract void TakeDamage();
}
