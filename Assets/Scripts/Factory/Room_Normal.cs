using System.Collections.Generic;
using UnityEngine;

public class Room_Normal : RoomObject
{
    [Header("Detection")]
    [SerializeField] private int rayLengthMultiplier = 8;
    [SerializeField] GameObject cameraObjectFollow;
    [SerializeField] private List<SpriteRenderer> roomSprites = new List<SpriteRenderer>();
    [SerializeField] private SpriteRenderer minimap;

    private bool checkForRooms;
    private bool once;

    public bool[] bools = new bool[4];

    public List<GameObject> PossibleLayouts = new List<GameObject>();
    public RoomScriptable RoomScriptable;

    private int cooldownTry = 3;
    void Awake()
    {
        foreach (GameObject wall in walls)
        {
            SpriteRenderer wallRenderer = wall.GetComponent<SpriteRenderer>();
            wallRenderer.sprite = RoomScriptable.S_wall;
            roomSprites.Add(wallRenderer);

            GameObject room = FindDoorOnObject(wall);

            if (room != null)
            {
                doors.Add(room);
                SpriteRenderer doorRenderer = room.GetComponent<SpriteRenderer>();
                doorRenderer.sprite = RoomScriptable.S_door;
                roomSprites.Add(doorRenderer);
            }
        }

        rayLengthMultiplier = RoomScriptable.RayLengthMultiplier;
        ground.sprite = RoomScriptable.S_ground;

        GameObject groundChild = FindGroundChild();
        if (groundChild != null)
        {
            SpriteRenderer groundChildRenderer = groundChild.GetComponent<SpriteRenderer>();
            if (groundChildRenderer != null)
            {
                roomSprites.Add(groundChildRenderer);
            }
        }

        if (ground.GetComponent<SpriteRenderer>() != null)
        {
            roomSprites.Add(ground.GetComponent<SpriteRenderer>());
        }

        cameraObjectFollow = GameObject.Find("CameraFollow");
    }

    private void Start()
    {
        cameraPosition = transform.localPosition;
        ChangeFloorColor();
        InitializeTraps();
    }

    public void Update()
    {
        if (GameManager.Instance.GenerationCompleted && !once) 
        {
            cooldownTry--;

            if (cooldownTry != 0) return;
            once = true;
            if (PossibleLayouts.Count < 1) return;

            bool ableToSpawn = false;
            if ((doors[0].activeSelf && doors[2].activeSelf) && (!doors[1].activeSelf && !doors[3].activeSelf))
            {
                PossibleLayouts[0].SetActive(true);
                ableToSpawn = true;
                return;
            }
            if ((doors[0].activeSelf && doors[3].activeSelf) && (!doors[1].activeSelf && !doors[2].activeSelf))
            {
                PossibleLayouts[1].SetActive(true);
                ableToSpawn = true;
                return;
            }
            if ((doors[1].activeSelf && doors[2].activeSelf) && (!doors[0].activeSelf && !doors[3].activeSelf))
            {
                PossibleLayouts[3].SetActive(true);
                ableToSpawn = true;
                return;

            }
            if ((doors[1].activeSelf && doors[3].activeSelf) && (!doors[0].activeSelf && !doors[2].activeSelf))
            {
                PossibleLayouts[2].SetActive(true);
                ableToSpawn = true;
                return;
            }
            if (!ableToSpawn)
                PossibleLayouts[4].SetActive(true);
        }

        if (!checkForRooms)
        {
            checkForRooms = true;
            CheckConnectedDoors();
        }
    }

    

    GameObject FindDoorOnObject(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == "Door")
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private GameObject FindGroundChild()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Ground"))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public override void EraseUncheckDoors()
    {
        foreach (GameObject door in doors)
        {
            DoorCheck doorCheck = door.GetComponent<DoorCheck>();
            if (doorCheck.ConnectedDoor == null && doorCheck.TryNumberTimes <= 0)
            {
                door.SetActive(false);
            }
        }

        cameraPosition = transform.localPosition;
    }

    public override void CheckConnectedDoors()
    {
        foreach (GameObject door in doors)
        {
            DoorCheck doorCheck = door.GetComponent<DoorCheck>();

            Vector3 direction = ParentDirection(door);
            doorCheck.SetDirectionAndDistance(direction, rayLengthMultiplier);
        }
    }

    public Vector3 ParentDirection(GameObject door)
    {
        if (door.transform.parent.gameObject.name == "UpWall") { return Vector3.forward; }
        if (door.transform.parent.gameObject.name == "DownWall") { return Vector3.back; }
        if (door.transform.parent.gameObject.name == "LeftWall") { return Vector3.left; }
        if (door.transform.parent.gameObject.name == "RightWall") { return Vector3.right; }

        return Vector3.zero;
    }

    public override void MoveCameraFollow()
    {
        GameManager.Instance.spawnEnemies.Spawning(this);
        cameraObjectFollow.transform.position = cameraPosition;
    }

    public List<SpriteRenderer> GetWallAndDoorSpriteRenderers()
    {
        return roomSprites;
    }

    private void ChangeFloorColor()
    {
        int iterations = GameManager.Instance.iterations;
        Color color = GenerateColor(iterations);

        foreach (SpriteRenderer renderer in roomSprites)
        {
            renderer.color = color;
        }
    }

    private Color GenerateColor(int iterations)
    {
        if (iterations == 0)
        {
            return Color.white;
        }
        else
        {
            float hue = Mathf.Repeat((iterations + 4) * 0.1f, 1f);
            float saturation = 0.3f;
            float value = 0.9f;

            return Color.HSVToRGB(hue, saturation, value);
        }
    }

    public void IsCurrentRoom(bool isActive)
    {
        if (minimap != null)
        {
            minimap.color = isActive ? Color.red : GenerateColor(GameManager.Instance.iterations);
        }
    }

}
