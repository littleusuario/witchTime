using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    [SerializeField] GameObject objectFollow;
    void Update()
    {
        Vector3 newPos = objectFollow.transform.position;
        newPos.y = 0f;

        transform.position = newPos;
    }
}
