using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [Header("Factory")]
    [SerializeField] RoomFactory roomFactory;
    [SerializeField] List<RoomObject> possibleRooms;
    [SerializeField] int maxNumberOfRooms = 15;
    [SerializeField] int currentRooms = 0;

    [Header("Room Variables")]
    [SerializeField] Vector3 roomPosition = Vector3.zero;
    [SerializeField] Transform RoomParent;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private int PossibilyToContinue = 5;
    
    [SerializeField] RoomObject rootRoom;
    [SerializeField] List<RoomObject> roomList = new List<RoomObject>();
    [SerializeField] SpawnEnemies spawnEnemies;
    bool doOnlyOncePlease;

    public int iterations = 0;
    public void Start()
    {
        iterations++;
        roomFactory = GetComponent<RoomFactory>();

        currentRooms = 0;

        if (RoomParent == null)
        {
            GameObject roomParent = new GameObject();
            roomParent.name = "RoomParent";
            RoomParent = roomParent.transform;
        }

        roomPosition = Vector3.zero;
        rootRoom = roomCreate(roomPosition);
        rootRoom.transform.parent = RoomParent;
        currentRooms++;
        roomList.Add(rootRoom);

        RoomGenerator();

        RoomObject depthestRoom = null;
        int depth = 0;

        foreach (RoomObject roomObject in roomList) 
        {
            roomObject.CheckDoors();
        }

        foreach (RoomObject roomObject in roomList)
        {
            CheckRoomDepth(roomObject, depth);
            if (roomObject.depth >= depth)
            {
                depthestRoom = roomObject;
                depth = roomObject.depth;
            }
        }


        //foreach (Room_Normal rooms in roomList)
        //{
        //    spawnEnemies.Notcleared.Add(rooms);
        //}



        Instantiate(exitPrefab, depthestRoom.transform.position + Vector3.up, Quaternion.Euler(90, 0, 0), depthestRoom.transform);
    }

 
    private void Update()
    {
        if (!doOnlyOncePlease) 
        {

            //doOnlyOncePlease = true;
            foreach (RoomObject roomObject in roomList)
            {
                roomObject.EraseUncheckDoors();
            }
        }
    }
    public void RoomGenerator()
    {
        while (currentRooms < maxNumberOfRooms)
        {
            int possibility = 0;
            possibility = Random.Range(0, 2);
            if (possibility == 1)
            {
                CheckRoom(rootRoom);
            }
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            int index = i;

            for (int j = 0; j < roomList.Count; j++)
            {
                if (roomList[j] != null)
                {
                    if (index > roomList.Count)
                    {
                        Destroy(roomList[j].gameObject);
                        roomList.RemoveAt(j);
                        currentRooms--;
                        break;
                    }
                    var objI = roomList[i];
                    var objJ = roomList[j];
                    if (objI != objJ)
                    {
                        if (roomList[i].transform.position == roomList[j].transform.position && roomList[j] != rootRoom)
                        {
                            Destroy(roomList[j].gameObject);
                            roomList.RemoveAt(j);
                            currentRooms--;
                            break;
                        }
                    }
                }
            }
        }

        if (currentRooms < maxNumberOfRooms)
        {
            RoomGenerator();
        }
    }

    private void CheckRoom(RoomObject roomToCheck)
    {
        int roomToChose = Random.Range(0, 4);

        if (roomToCheck != null)
        {
            switch (roomToChose)
            {
                case 0:
                    CheckDoors(roomToChose, new Vector3(0, 0, 17), roomToCheck);
                    break;
                case 1:
                    CheckDoors(roomToChose, new Vector3(0, 0, -17), roomToCheck);
                    break;
                case 2:
                    CheckDoors(roomToChose, new Vector3(-17, 0, 0), roomToCheck);
                    break;
                case 3:
                    CheckDoors(roomToChose, new Vector3(17, 0, 0), roomToCheck);
                    break;

                case 4:
                    break;

                default:
                    break;
            }
        }
    }

    private RoomObject roomCreate(Vector3 desiredPosition)
    {
        RoomObject room = null;

        if (roomList.Count > 0) 
        {
            int possibility = Random.Range(0, possibleRooms.Count);

            room = roomFactory.RoomCreator(possibleRooms[possibility].ID);
        }
        else 
        {
            room = roomFactory.RoomCreator("Normal");
        }
        room.gameObject.transform.position = desiredPosition;
        return room;
    }

    public void CheckDoors(int index, Vector3 direction, RoomObject roomToCheck)
    {
        if (roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom == null)
        {
            Vector3 desiredPosition = roomToCheck.transform.position + direction;
            RoomObject newRoom = roomCreate(desiredPosition);
            newRoom.transform.parent = RoomParent;
            roomList.Add(newRoom);
            roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom = newRoom;

            currentRooms++;
            return;
        }
        else
        {
            CheckRoom(roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom);
        }
    }

    private void CheckRoomDepth(RoomObject room, int depth)
    {
        if (room == null) return;

        if (depth > room.depth)
        {
            depth++;
            room.depth = depth;
        }

        RoomObject upRoom = room.doors[0].GetComponent<DoorCheck>().ConnectedRoom;
        RoomObject downRoom = room.doors[1].GetComponent<DoorCheck>().ConnectedRoom;
        RoomObject rightRoom = room.doors[2].GetComponent<DoorCheck>().ConnectedRoom;
        RoomObject leftRoom = room.doors[3].GetComponent<DoorCheck>().ConnectedRoom;

        //if (upRoom != null)
        //    CheckRoomDepth(upRoom, depth);
        
        //if (downRoom != null)
        //    CheckRoomDepth(downRoom, depth);
        
        //if (rightRoom != null)
        //    CheckRoomDepth(rightRoom, depth);
        
        //if (leftRoom != null)
        //    CheckRoomDepth(leftRoom, depth);
    }

}
