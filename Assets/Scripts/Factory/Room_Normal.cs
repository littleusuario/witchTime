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
    public RoomScriptable RoomScriptable;
    public List<GameObject> EnemiestoSpawn = new List<GameObject>();
    public int NumberOfenemies;
    public Transform enemyspawn;
    void Awake()
    {
        NumberOfenemies = EnemiestoSpawn.Count;
        foreach (GameObject wall in walls) 
        {
            wall.GetComponent<SpriteRenderer>().sprite = RoomScriptable.S_wall;
            GameObject room = FindDoorOnObject(wall);

            if(room != null) 
            {
                doors.Add(room);
                room.GetComponent<SpriteRenderer>().sprite = RoomScriptable.S_door;
            }
        }

        rayLengthMultiplier = RoomScriptable.RayLengthMultiplier;
        ground.sprite = RoomScriptable.S_ground;
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

            CheckDoors();
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

    public override void CheckDoors()
    {
        foreach (GameObject door in doors)
        {
            DoorCheck doorCheck = door.GetComponent<DoorCheck>();

            Vector3 direction = ParentDirection(door);
            doorCheck.CheckForDoor(direction, rayLengthMultiplier);             
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
