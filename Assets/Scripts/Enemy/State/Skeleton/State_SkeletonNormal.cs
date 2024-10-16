using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SkeletonNormal : State_EnemyNormal
{
    FirstEnemy enemy;
    public State_SkeletonNormal(Enemy enemy)
    {
        this.enemy = (FirstEnemy)enemy;
    }

    public override void UpdateState()
    {
        enemy.Enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.Player.transform.position, enemy._StepDistance);

        if(enemy.HealthPoints <= 0) 
        {
            StateManager.SwitchState(StateManager.enemyDeathState);
        }
    }
}
