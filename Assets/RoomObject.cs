using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObject : MonoBehaviour
{
    public List<GameObject> walls = new List<GameObject>();

    public List<GameObject> doors = new List<GameObject>();

    public Vector3 cameraPosition;
}
