using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    public List<RoomObject> Notcleared = new List<RoomObject>();
    public List<RoomObject> ClearedRooms = new List<RoomObject>();
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.Instance.ActualRoom == Notcleared.Contains(GameManager.Instance.ActualRoom))
        //{

        //    Room_Normal spawn = GameManager.Instance.ActualRoom.GetComponentInChildren<Room_Normal>();
        //    for (int i = 0; i < spawn.NumberOfenemies; i++)
        //    {
        //        Instantiate(spawn.EnemiestoSpawn[i]);
        //    }
           
        //}
    }
}
