using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Vector3 roomPosition;
    [SerializeField] RoomFactory roomFactory;
    [SerializeField] int maxNumberOfRooms = 15;
    [SerializeField] int currentRooms = 0;
    [SerializeField] RoomObject rootRoom;
    [SerializeField] List<RoomObject> roomList;
    [SerializeField] Transform RoomParent;
    event Action startCheck;
    [SerializeField] private int PossibilyToContinue = 5;

    public int iterations = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if
        (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rootRoom = roomCreate(roomPosition);
        rootRoom.transform.parent = RoomParent;
        currentRooms++;
        roomList.Add(rootRoom);

        RoomGenerator();
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
                    var objI = roomList[i];
                    var objJ = roomList[j];
                    if (objI != objJ)
                    {
                        if (roomList[i].transform.position == roomList[j].transform.position)
                        {
                            Destroy(roomList[j].gameObject);
                            roomList.RemoveAt(j);
                            currentRooms--;
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
                    Debug.Log("Limit");

                    break;

                default: 
                
                    break;
            }
            //roomToCheck.StartCheckDoors();
        }
    }

    private RoomObject roomCreate(Vector3 desiredPosition)
    {
        RoomObject room = roomFactory.RoomCreator("Normal");
        room.gameObject.transform.position = desiredPosition;
        return room;
    }

    public void CheckDoors(int index, Vector3 direction, RoomObject roomToCheck)
    {
        if (roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom == null)
        {
            Debug.Log("ThereIsNothing");
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
            int possibility = Random.Range(0, PossibilyToContinue);
            if (possibility < PossibilyToContinue / 4)
            {
                Debug.Log("ThereIsSomething");
                CheckRoom(roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom);
            return;
            }
            else 
            {
                index--;
                if (index < 0) 
                {
                    index = 3;
                }

                CheckRoom(roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom);
            }
        }
    }
}
