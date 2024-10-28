using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Room_Normal : RoomObject
{
    [Header("Detection")]
    [SerializeField] private int rayLengthMultiplier = 8;
    private GameObject cameraObjectFollow;
    private bool checkForRooms = false;

    private void Awake()
    {
        cameraObjectFollow = GameObject.Find("CameraFollow");
    }

    private void Start()
    {
        if (roomData != null)
        {
            MoveCameraFollow();
        }
    }

    private void Update()
    {
        if (!checkForRooms)
        {
            checkForRooms = true;
            CheckDoors();
            EraseUncheckDoors();
        }
    }

    private void CheckDoors()
    {
        foreach (GameObject door in roomData.doors)
        {
            DoorCheck doorCheck = door.GetComponent<DoorCheck>();
            Vector3 direction = ParentDirection(door);
            doorCheck.CheckForDoor(direction, rayLengthMultiplier);
        }
    }

    private Vector3 ParentDirection(GameObject door)
    {
        string parentName = door.transform.parent.gameObject.name;
        if (parentName == "UpWall") return Vector3.forward;
        if (parentName == "DownWall") return Vector3.back;
        if (parentName == "LeftWall") return Vector3.left;
        if (parentName == "RightWall") return Vector3.right;

        return Vector3.zero;
    }

    public override void MoveCameraFollow()
    {
        if (cameraObjectFollow != null)
        {
            cameraObjectFollow.transform.position = roomData.defaultCameraPosition;
        }
    }

    public override void EraseUncheckDoors()
    {
        foreach (GameObject door in roomData.doors)
        {
            if (door.GetComponent<DoorCheck>().ConnectedRoom == null)
            {
                door.SetActive(false);
            }
        }
    }
}