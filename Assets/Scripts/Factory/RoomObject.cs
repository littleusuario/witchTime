using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour
{
    [Header("Room Walls")]

    public string ID;

    public List<GameObject> walls = new List<GameObject>();

    public List<GameObject> doors = new List<GameObject>();

    public SpriteRenderer ground;

    public Vector3 cameraPosition;

    public int depth = 0;
    public bool RoomChecked;
    public List<GameObject> EnemiestoSpawn = new List<GameObject>();
    public List<TrapObject> Traps = new List<TrapObject>();

    public void InitializeTraps() 
    {
        foreach (Transform child in transform) 
        {
            TrapObject trap = child.GetComponent<TrapObject>();
            
            if (trap != null) 
            {
                Traps.Add(trap);
            }
        }


        foreach (TrapObject trap in Traps) 
        {
            trap.OriginRoom = this;
        }
    }
    public virtual void CheckDoors() { }
    public virtual void MoveCameraFollow() { }

    public virtual void StartCheckDoors() { }

    public virtual void EraseUncheckDoors() { }

}

