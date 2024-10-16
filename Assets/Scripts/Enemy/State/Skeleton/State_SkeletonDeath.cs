using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SkeletonDeath : State_EnemyDeath
{
    FirstEnemy enemy;
    public State_SkeletonDeath(Enemy enemy)
    {
        this.enemy = (FirstEnemy)enemy;
    }

    public override void EnterState()
    {
        enemy.Death = true;
        enemy.Animator.SetBool("Death", true);
    }
}
