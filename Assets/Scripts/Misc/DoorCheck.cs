using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] RoomObject connectedRoom;
    [SerializeField] RoomObject originRoom;
    [SerializeField] DoorCheck connectedDoor;
    [SerializeField] List<Collider> colliders;
    [SerializeField] Vector3 direction;
    [SerializeField] float threshold = 1.0f;
    private float elapsedTime = 0;
    [SerializeField] float distance;

    [SerializeField] AudioClip doorSound;
    private AudioSource audioSource;

    private float minPitch = 0.8f;
    private float maxPitch = 1.3f;

    public RoomObject ConnectedRoom { get => connectedRoom; set => connectedRoom = value; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }

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

        elapsedTime += Time.deltaTime;
        distance = Vector3.Distance(transform.position, Player.transform.position);
        if  (distance <= threshold)
        {
            elapsedTime = 0f;
            connectedRoom.MoveCameraFollow();
            Vector3 newPosition = connectedDoor.transform.position + -connectedDoor.direction.normalized * 1.5f;
            newPosition.y = 0f;
            Player.transform.position = newPosition;

            if (audioSource != null && doorSound != null)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(doorSound);
            }
        }
    }
}
