using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Flyweight/RoomData")]
public class RoomData : ScriptableObject
{
    public string roomID;
    public List<Sprite> walls;
    public List<GameObject> doors;
    public Vector3 defaultCameraPosition;
}

