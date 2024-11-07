using UnityEngine;

[CreateAssetMenu(fileName = "RoomScriptable")]
public class RoomScriptable : ScriptableObject
{
    public string roomID;
    public Sprite S_wall;
    public Sprite S_door;
    public Sprite S_ground;
    public int RayLengthMultiplier;
}
