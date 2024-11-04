using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [SerializeField] List<RoomObject> roomList = new List<RoomObject>();
    [SerializeField] Dictionary<string, RoomObject> roomDictionary = new Dictionary<string, RoomObject>();

    private void Awake()
    {
        foreach (RoomObject roomObject in roomList) 
        {
            roomDictionary.Add(roomObject.ID, roomObject);
        }
    }

    public RoomObject RoomCreator(string id) 
    {
        if (roomDictionary.TryGetValue(id, out RoomObject room)) 
        {
            return Instantiate(room);
            
        }

        return null;
    }
}
