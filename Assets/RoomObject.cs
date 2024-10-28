using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour
{
    [Header("Room Walls")]
    public string ID;

    [Header("Room Data")]
    public RoomData roomData;

    [Header("Instance-specific Data")]
    public Vector3 cameraPosiion;

    public void Initialize(RoomData data, Vector3 position)
    {
        this.roomData = data;
        this.cameraPosiion = position;
        transform.position = position;
    }

    public virtual void MoveCameraFollow()
    {
        Camera.main.transform.position = roomData.defaultCameraPosition;
    }

    public int depth = 0;

    public virtual void StartCheckDoors() { }

    public virtual void EraseUncheckDoors() { }
}

