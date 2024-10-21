using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room_Normal : RoomObject
{
    [Header("Detection")]
    [SerializeField] private int rayLengthMultiplier = 8;
    [SerializeField] GameObject cameraObjectFollow;
    void Start()
    {
        foreach (GameObject wall in walls) 
        {
            GameObject room = FindDoorOnObject(wall);

            if(room != null) 
            {
                doors.Add(room);
            }
        }

        foreach (GameObject door in doors) 
        {
            CheckDoors(door);
        }
        cameraObjectFollow = GameObject.Find("CameraFollow");
        cameraPosition = transform.localPosition;
    }

    GameObject FindDoorOnObject(GameObject parent) 
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == "Door")
            {
                return child.gameObject;
            }
        }

        return null;
    }

    void CheckDoors(GameObject door) 
    {
        Vector3 direction = ParentDirection(door);
        DoorCheck doorCheck = door.GetComponent<DoorCheck>();
        GameObject connectedDoor = doorCheck.CheckForDoor(direction, rayLengthMultiplier);

        if (connectedDoor == null) 
        {
            door.SetActive(false);
        }
    }

    public Vector3 ParentDirection(GameObject door) 
    {
        if (door.transform.parent.gameObject.name == "UpWall") { return Vector3.forward; }
        if (door.transform.parent.gameObject.name == "DownWall") { return Vector3.back; }
        if (door.transform.parent.gameObject.name == "LeftWall") { return Vector3.left; }
        if (door.transform.parent.gameObject.name == "RightWall") { return Vector3.right; }

        return Vector3.zero;
    }

    public override void MoveCameraFollow()
    {
        cameraObjectFollow.transform.position = cameraPosition;
    }
}
