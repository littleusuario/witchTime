using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] RoomObject connectedRoom;
    [SerializeField] RoomObject originRoom;
    [SerializeField] DoorCheck connectedDoor;
    [SerializeField] List<Collider> colliders;
    [SerializeField] Vector3 direction;
    
    public RoomObject ConnectedRoom { get => connectedRoom; set => connectedRoom = value; }

    public GameObject CheckForDoor(Vector3 direction, float maxDistance) 
    {
        this.direction = direction; 
        RaycastHit[] hits;
        Debug.DrawRay(transform.position, direction * maxDistance, Color.white, 1f);

        hits = Physics.RaycastAll(transform.position, direction, maxDistance);

        foreach (RaycastHit rayHit in hits) 
        {
            colliders.Add(rayHit.collider);
            if (rayHit.collider.transform.gameObject.CompareTag("Door"))
            {
                GameObject hitGameObject = rayHit.collider.transform.gameObject;

                if (hitGameObject != gameObject) 
                {
                    Debug.DrawRay(transform.position, direction * maxDistance, Color.magenta, 1f);
                    GameObject otherDoor = hitGameObject;
                    connectedDoor = hitGameObject.GetComponent<DoorCheck>();

                    connectedRoom = connectedDoor.originRoom;
                    return hitGameObject;
                }
            }
        }
        return null;
    }

    public void Update()
    {
        if(Physics.Raycast(transform.position, -direction.normalized, out RaycastHit hit, 1f)) 
        {
            if (hit.collider.transform.gameObject.CompareTag("Player")) 
            {
                connectedRoom.MoveCameraFollow();
                hit.collider.transform.gameObject.transform.position = connectedDoor.transform.position + -connectedDoor.direction.normalized * 1.5f;
            }
        }
    }
}
