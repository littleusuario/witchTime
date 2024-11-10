using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapObject : MonoBehaviour
{
    protected GameObject player;
    protected RoomObject originRoom;
    public RoomObject OriginRoom { get => originRoom; set => originRoom = value; }
    protected void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public abstract void TrapActivate();
}
