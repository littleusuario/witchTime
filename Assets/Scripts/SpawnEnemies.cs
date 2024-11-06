using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<RoomObject> Notcleared = new List<RoomObject>();

    public void Spawning( Room_Normal ActualRoom)
    {
        if (Notcleared.Contains(ActualRoom))
        {
            if (GameManager.Instance.ActualRoom != null) 
            {
                Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();        
            
                foreach (GameObject enemy in spawn.EnemiestoSpawn) 
                {
                    enemy.SetActive(true);
                }
                spawn.EnemiestoSpawn.Clear();
            }
        }
    }

    private void Update()
    {
        clear();
    }

   private void clear()
    {
        if (GameManager.Instance.ActualRoom != null) 
        {
            Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponent<Room_Normal>();
            if (spawn.EnemiestoSpawn.Count == 0)
            {
                Notcleared.Remove(GameManager.Instance.ActualRoom);
            }
        }
    }
}
