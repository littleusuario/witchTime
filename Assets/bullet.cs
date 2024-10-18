using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 1f;  // Defines how long before the bullet is destroyed




  
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
   
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
       
   

        if (timer > bulletLife)
        {
            Destroy(gameObject);
            timer = 0f;
        }
    }


   
}

