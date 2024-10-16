using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    public Enemy CreateEnemy(string enemyType)
    {
        switch (enemyType)
        {
            case "firstenemy":
                return new FirstEnemy();
            case "secondenemy":
                return new SecondEnemy();
            default:
                return null;
        }
    }
}
