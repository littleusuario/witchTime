using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private RoomObject connectedRoom;
    [SerializeField] private RoomObject originRoom;
    [SerializeField] private DoorCheck connectedDoor;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float threshold = 1.0f;
    [SerializeField] private float distance;
    [SerializeField] private AudioClip doorSound;
    
    private float elapsedTime = 0;
    private AudioSource audioSource;
    private float minPitch = 0.8f;
    private float maxPitch = 1.3f;
    private float maxDistance;
    private int tryNumberTimes = 2;
    private Animator animator;
    private bool noEnemies = false;
    private BeatManager beatManager;

    public RoomObject ConnectedRoom { get => connectedRoom; set => connectedRoom = value; }
    public DoorCheck ConnectedDoor { get => connectedDoor; set => connectedDoor = value; }
    public int TryNumberTimes => tryNumberTimes;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        GameObject gameObject = GameObject.FindGameObjectWithTag("BeatManager");
        beatManager = gameObject.GetComponent<BeatManager>();
        beatManager.Intervals[0]._Trigger.AddListener(CheckEnemies);
    }

    public void SetDirectionAndDistance(Vector3 direction, float maxDistance)
    {
        this.direction = direction;
        this.maxDistance = maxDistance;
    }

    private void CheckEnemies() 
    {
        if (originRoom.EnemiestoSpawn.Count == 0 && GameManager.Instance.ActualRoom == originRoom)
        {
            animator.SetBool("NoEnemies", true);
            noEnemies = true;
        }
    }
    private void FixedUpdate()
    {
        if (direction != Vector3.zero && maxDistance != 0 && tryNumberTimes > 0) 
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

            tryNumberTimes--;
        }
    }
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        distance = Vector3.Distance(transform.position, Player.transform.position);
        if  (distance <= threshold && noEnemies)
        {
            elapsedTime = 0f;
            GameManager.Instance.SetCurrentRoom(connectedRoom);
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
