using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] FacadeManager facade;

    public int iterations = 0;
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
            facade = null;
        }

        DontDestroyOnLoad(gameObject);

        if (Instance == this) 
        {
            facade.StartGame();
        }
    }

    public void LoadNextLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
