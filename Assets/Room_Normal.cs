using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room_Normal : RoomObject
{
    [Header("Detection")]
    [SerializeField] private int rayLengthMultiplier = 8;
    [SerializeField] GameObject cameraObjectFollow;
    bool checkForRooms;
    void Awake()
    {
        foreach (GameObject wall in walls) 
        {
            GameObject room = FindDoorOnObject(wall);

            if(room != null) 
            {
                doors.Add(room);
            }
        }
        cameraObjectFollow = GameObject.Find("CameraFollow");
    }

    private void Start()
    {
        cameraPosition = transform.localPosition;
    }

    public void Update()
    {
        if (!checkForRooms) 
        {
            checkForRooms = true;

            foreach (GameObject door in doors)
            {
                CheckDoors(/*door*/);
                EraseUncheckDoors();
            }
        }
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

    public override void EraseUncheckDoors()
    {
        foreach (GameObject door in doors)
        {
            if (door.GetComponent<DoorCheck>().ConnectedRoom == null)
            {
                door.SetActive(false);
            }
        }

        cameraPosition = transform.localPosition;
    }

    public void CheckDoors()
    {
        foreach (GameObject door in doors)
        {
            DoorCheck doorCheck = door.GetComponent<DoorCheck>();

            Vector3 direction = ParentDirection(door);
            GameObject connectedDoor = doorCheck.CheckForDoor(direction, rayLengthMultiplier);

            //if (connectedDoor == null)
            //{
            //    door.gameObject.SetActive(false);
            //}
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
