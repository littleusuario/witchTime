using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else if 
        (Instance != this) 
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Destroy(this);
    }

    private void OnDestroy()
    {
        Debug.Log("Hola soy un game m-");
    }
}
