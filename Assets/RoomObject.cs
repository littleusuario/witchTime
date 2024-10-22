using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour
{
    [Header("Room Walls")]

    public string ID;

    public List<GameObject> walls = new List<GameObject>();

    public List<GameObject> doors = new List<GameObject>();

    public Vector3 cameraPosition;

    public virtual void MoveCameraFollow() { }

    public virtual void StartCheckDoors() { }

    public virtual void EraseUncheckDoors() { }
}

