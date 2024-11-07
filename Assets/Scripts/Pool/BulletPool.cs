using System.Collections.Generic;
using UnityEngine;

public class bulletpool : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private int initialsize = 10;
    private List<GameObject> bullets = new List<GameObject>();
    void Awake()
    {
        addBulletsToPool(initialsize);
    }

    private void addBulletsToPool(int initialsize)
    {
        Instantiate(bullet);
    }
}
