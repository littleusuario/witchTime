using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
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
