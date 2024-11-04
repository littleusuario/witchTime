using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<RoomObject> Notcleared = new List<RoomObject>();

    Room_Normal spawn;
    public void Spawning()
    {

        if (GameManager.Instance.ActualRoom == Notcleared.Contains(GameManager.Instance.ActualRoom))
        {
            Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();
            for (int i = 0; i < spawn.NumberOfenemies; i++)
            {
                spawn.EnemiestoSpawn[i].gameObject.SetActive(true);
            }
        }

    }

    public void clear()
    {
        Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();
        if (spawn.EnemiestoSpawn.Count == 0) { 

            Notcleared.Remove(GameManager.Instance.ActualRoom);
        }
    }
}
