using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Vector2 offset;

    private static CursorController instance;
    private bool isClicked = false;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(hit.point.x + offset.x, 0, hit.point.z + offset.y);
        }
    }

    public static bool IsMouseClicking()
    {
        if (instance == null)
            return false;

        return instance.isClicked;
    }
}
