using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Vector3 roomPosition;
    [SerializeField] RoomFactory roomFactory;
    [SerializeField] int maxNumberOfRooms = 15;
    [SerializeField] int currentRooms = 0;
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

    private void Start()s
    {
        while (currentRooms < maxNumberOfRooms) 
        {
            GameObject room = roomFactory.RoomCreator("Normal").gameObject;
            room.transform.position = roomPosition * (currentRooms + 1);
            currentRooms++;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Hola soy un game m-");
    }
}
