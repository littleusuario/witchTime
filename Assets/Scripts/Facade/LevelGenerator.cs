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
    [SerializeField] Vector3 roomPosition;
    [SerializeField] Transform RoomParent;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private int PossibilyToContinue = 5;
    
    [SerializeField] RoomObject rootRoom;
    [SerializeField] List<RoomObject> roomList;

    public int iterations = 0;
    public void CreateLevelProcess(Scene scene, LoadSceneMode loadSceneMode)
    {
        iterations++;
        foreach (RoomObject room in roomList) 
        {
            Destroy(room);
        }
        roomList.Clear();
        currentRooms = 0;
        //if (iterations > 1)
        //{
        //    return;
        //}

        if (RoomParent == null)
        {
            GameObject roomParent = new GameObject();
            roomParent.name = "RoomParent";
            RoomParent = roomParent.transform;
        }


        rootRoom = roomCreate(roomPosition);
        rootRoom.transform.parent = RoomParent;
        currentRooms++;
        roomList.Add(rootRoom);

        RoomGenerator();

        RoomObject depthestRoom = null;
        int depth = 0;

        foreach (RoomObject roomObject in roomList)
        {
            CheckRoomDepth(roomObject, depth);
            if (roomObject.depth >= depth)
            {
                depthestRoom = roomObject;
                depth = roomObject.depth;
            }
        }
        foreach (RoomObject roomObject in roomList) 
        {
            roomObject.CheckDoors();
        }
        Instantiate(exitPrefab, depthestRoom.transform.position + Vector3.up, Quaternion.Euler(90, 0, 0), depthestRoom.transform);
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
                    //Debug.Log("Limit");
                    break;

                default:
                    //Debug.Log("Out");
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
            //depth = room.depth;
        }
        CheckRoomDepth(room.doors[0].GetComponent<DoorCheck>().ConnectedRoom, depth);
        CheckRoomDepth(room.doors[1].GetComponent<DoorCheck>().ConnectedRoom, depth);
        CheckRoomDepth(room.doors[2].GetComponent<DoorCheck>().ConnectedRoom, depth);
        CheckRoomDepth(room.doors[3].GetComponent<DoorCheck>().ConnectedRoom, depth);
    }

}
