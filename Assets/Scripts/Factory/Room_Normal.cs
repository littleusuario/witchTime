using System.Collections.Generic;
using UnityEngine;

public class Room_Normal : RoomObject
{
    [Header("Detection")]
    [SerializeField] private int rayLengthMultiplier = 8;
    [SerializeField] GameObject cameraObjectFollow;
    [SerializeField] private List<SpriteRenderer> roomSprites = new List<SpriteRenderer>();
    [SerializeField] private SpriteRenderer minimap;
    [SerializeField] GameObject DecorationPrefab;

    private GameObject decorationObject;
    private bool checkForRooms;
    private bool checkEnemiesAndLayouts;
    private bool decoration;

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

        foreach (GameObject layout in PossibleLayouts) 
        {
            foreach(Transform child in layout.gameObject.transform)
            {
                if (child.gameObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                    roomSprites.Add(spriteRenderer);
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
        if (GameManager.Instance.GenerationCompleted && !checkEnemiesAndLayouts)
        {
            cooldownTry--;

            if (cooldownTry != 0) return;
            checkEnemiesAndLayouts = true;
            if (PossibleLayouts.Count < 1) return;

            bool ableToSpawn = false;
            if ((doors[0].activeSelf && doors[2].activeSelf) && (!doors[1].activeSelf && !doors[3].activeSelf))
            {
                PossibleLayouts[0].SetActive(true);
                CheckEnemiesLayout();
                ableToSpawn = true;
                return;
            }
            if ((doors[0].activeSelf && doors[3].activeSelf) && (!doors[1].activeSelf && !doors[2].activeSelf))
            {
                PossibleLayouts[1].SetActive(true);
                CheckEnemiesLayout();
                ableToSpawn = true;
                return;
            }
            if ((doors[1].activeSelf && doors[2].activeSelf) && (!doors[0].activeSelf && !doors[3].activeSelf))
            {
                PossibleLayouts[3].SetActive(true);
                CheckEnemiesLayout();
                ableToSpawn = true;
                return;

            }
            if ((doors[1].activeSelf && doors[3].activeSelf) && (!doors[0].activeSelf && !doors[2].activeSelf))
            {
                PossibleLayouts[2].SetActive(true);
                CheckEnemiesLayout();
                ableToSpawn = true;
                return;
            }
            if (!ableToSpawn) 
            {
                PossibleLayouts[4].SetActive(true);
                CheckEnemiesLayout();
            }
        }

        if (!checkForRooms)
        {
            checkForRooms = true;
            CheckConnectedDoors();
        }
    }

    private void CheckEnemiesLayout()
    {
        bool noChange = true;

        while (noChange) 
        {
            noChange = false;
            for (int i = 0; i < EnemiestoSpawn.Count; i++)
            {
                if (!EnemiestoSpawn[i].gameObject.transform.parent.gameObject.activeSelf)
                {
                    EnemiestoSpawn.RemoveAt(i);
                    noChange = true;
                }
            }
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
                if (decorationObject == null && door.transform.parent.name != "DownWall")
                {
                    if (Random.value <= 0.25f && !decoration)
                    {
                        decorationObject = Instantiate(DecorationPrefab, door.transform.position, door.transform.rotation, door.transform.parent);
                        decorationObject.transform.position += new Vector3(0, Random.Range(0.25f, 0.35f), 0);
                        roomSprites.Add(decorationObject.GetComponent<SpriteRenderer>());
                        ChangeFloorColor();
                    }

                    decoration = true;
                }
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
