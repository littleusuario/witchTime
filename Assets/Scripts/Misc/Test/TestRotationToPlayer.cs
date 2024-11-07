using UnityEngine;

public class TestRotationToPlayer : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (player != null) 
        {
            transform.LookAt(player.transform.position);
            Vector3 pruebaPT = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            transform.eulerAngles = new Vector3(-pruebaPT.y - 90, -pruebaPT.y, 0);
        }
    }
}
