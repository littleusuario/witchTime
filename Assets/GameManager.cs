using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject exitPrefab;

    public int iterations = 0;
    private int depth = 0;
    private void Awake()
    {
        SceneManager.sceneLoaded += CreateLevelProcess;
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
    
    private void OnLevelWasLoaded(int level)
    {

    }
    private void CreateLevelProcess(Scene scene, LoadSceneMode loadSceneMode) 
    {
        iterations++;
        roomList.Clear();
        currentRooms = 0;
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

        if (iterations > 1)
        {
            return;
        }

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
                    Debug.Log("Limit");

                    break;

                default:

                    Debug.Log("Out");
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
            //int possibility = Random.Range(0, PossibilyToContinue);
            //if (possibility > PossibilyToContinue / 5)
            //{
                Debug.Log("ThereIsSomething");
                CheckRoom(roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom);
            //}
            //else 
            //{
            //    index--;
            //    if (index < 0) 
            //    {
            //        index = 3;
            //    }

            //    CheckRoom(roomToCheck.doors[index].GetComponent<DoorCheck>().ConnectedRoom);
            //}
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

    public void LoadNextLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
