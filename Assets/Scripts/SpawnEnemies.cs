using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<RoomObject> Notcleared = new List<RoomObject>();

    Room_Normal spawn;
    public void Spawning( Room_Normal ActualRoom)
    {

        if (Notcleared.Contains(ActualRoom))
        {
            Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();
            for (int i = 0; i < spawn.EnemiestoSpawn.Count; i++)
            {
                spawn.EnemiestoSpawn[i].gameObject.SetActive(true);
            }
        }
    

    }

    private void Update()
    {
        clear();
    }

   private void clear()
    {
        Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();
        if (spawn.EnemiestoSpawn.Count == 0) { 

            Notcleared.Remove(GameManager.Instance.ActualRoom);
        }
    }
}
