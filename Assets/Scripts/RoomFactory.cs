using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [SerializeField] private List<RoomData> roomDataList = new List<RoomData>();
    [SerializeField] private GameObject roomPrefab;
    private Dictionary<string, RoomData> roomDataDictionary = new Dictionary<string, RoomData>();

    private void Awake()
    {
        foreach (RoomData roomData in roomDataList)
        {
            roomDataDictionary[roomData.roomID] = roomData;
        }
    }

    public RoomObject RoomCreator(string id, Vector3 position)
    {
        if (roomDataDictionary.TryGetValue(id, out RoomData roomData))
        {
            RoomObject roomInstance = Instantiate(roomPrefab, position, Quaternion.identity).GetComponent<RoomObject>();
            roomInstance.Initialize(roomData, position);
            return roomInstance;
        }
        return null;
    }
}
