using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        EnemyFactory factory = new EnemyFactory();
        Enemy firstenemy = factory.CreateEnemy("first enemy");
        firstenemy.RunBehaviour();

        Enemy secondenemy = factory.CreateEnemy("second enemy");
        secondenemy.RunBehaviour();
    }
}
