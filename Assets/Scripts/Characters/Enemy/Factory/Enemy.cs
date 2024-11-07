using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Parent Class")]
    public int HealthPoints;
    public int AttackRadius = 5;
    public abstract event Action Ondie;
    public ParticleSystem DamageParticleSystem;
    public int beatsInactiveStart = 1;
    public int beats = 0;
    public abstract void RunBehaviour();
    public abstract void DamagaZone();
    public abstract void TakeDamage(int damage);
}
