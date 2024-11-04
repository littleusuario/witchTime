using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<RoomObject> Notcleared = new List<RoomObject>();
   
    public void Spawning()
    {

        if (GameManager.Instance.ActualRoom == Notcleared.Contains(GameManager.Instance.ActualRoom))
        {
            Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();
            for (int i = 0; i < spawn.NumberOfenemies; i++)
            {
                Instantiate(spawn.EnemiestoSpawn[i],spawn.enemyspawn);
            }
        }

    }
}
