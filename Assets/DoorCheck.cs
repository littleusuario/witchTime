using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] GameObject ConnectedRoom;
    [SerializeField] List<Collider> colliders;
    public GameObject CheckForDoor(Vector3 direction, float maxDistance) 
    {
        RaycastHit[] hits;
        Debug.DrawRay(transform.position, direction * maxDistance, Color.white, 99f);

        hits = Physics.RaycastAll(transform.position, direction, maxDistance);

        foreach (RaycastHit rayHit in hits) 
        {
            colliders.Add(rayHit.collider);
            if (rayHit.collider.transform.gameObject.CompareTag("Door"))
            {
                GameObject hitGameObject = rayHit.collider.transform.gameObject;

                if (hitGameObject != gameObject) 
                {
                    Debug.DrawRay(transform.position, direction * maxDistance, Color.magenta, 99f);
                    ConnectedRoom = hitGameObject.transform.parent.gameObject;
                    return hitGameObject;
                }
            }
        }
        return null;
    }
}
