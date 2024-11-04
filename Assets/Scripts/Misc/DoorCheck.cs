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
    private float maxDistance;

    public RoomObject ConnectedRoom { get => connectedRoom; set => connectedRoom = value; }
    public DoorCheck ConnectedDoor { get => connectedDoor; set => connectedDoor = value; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();

    }

    public GameObject CheckForDoor(Vector3 direction, float maxDistance)
    {
        this.direction = direction;
        this.maxDistance = maxDistance;
        RaycastHit[] hits = null;
        Debug.DrawRay(transform.position, direction * maxDistance, Color.white, 1f);

        hits = Physics.RaycastAll(transform.position, direction, maxDistance);

        if (hits.Length == 0) { }

        foreach (RaycastHit rayHit in hits)
        {
            colliders.Add(rayHit.collider);
            if (rayHit.collider.transform.gameObject.CompareTag("Door"))
            {
                GameObject hitGameObject = rayHit.collider.transform.gameObject;

                if (hitGameObject != gameObject)
                {
                    Debug.DrawRay(transform.position, direction * maxDistance, Color.magenta, 1f);
                    connectedDoor = hitGameObject.GetComponent<DoorCheck>();

                    connectedRoom = connectedDoor.originRoom;
                    return hitGameObject;
                }
            }
        }
        return null;
    }

    private void FixedUpdate()
    {
        if (direction != Vector3.zero && maxDistance != 0) 
        {
            RaycastHit[] hits = null;
            Debug.DrawRay(transform.position, direction * maxDistance, Color.white, 1f);

            hits = Physics.RaycastAll(transform.position, direction, maxDistance);

            if (hits.Length == 0) { }

            foreach (RaycastHit rayHit in hits)
            {
                colliders.Add(rayHit.collider);
                if (rayHit.collider.transform.gameObject.CompareTag("Door"))
                {
                    GameObject hitGameObject = rayHit.collider.transform.gameObject;

                    if (hitGameObject != gameObject)
                    {
                        Debug.DrawRay(transform.position, direction * maxDistance, Color.magenta, 1f);
                        connectedDoor = hitGameObject.GetComponent<DoorCheck>();

                        connectedRoom = connectedDoor.originRoom;
                    }
                }
            }

        }
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
            GameManager.Instance.ActualRoom = connectedRoom;
            GameManager.Instance.spawnEnemies.Spawning();
            if (audioSource != null && doorSound != null)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(doorSound);
            }
          
        
        }
    }
}
