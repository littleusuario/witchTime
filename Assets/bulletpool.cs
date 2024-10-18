using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletpool : MonoBehaviour
{
    // Start is called before the first frame update

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
