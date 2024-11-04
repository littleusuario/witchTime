using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private FacadeManager facade;
    public SpawnEnemies spawnEnemies;

    public int iterations = 0;

    [Header("Player Health")]
    [SerializeField] private int playerMaxHealth = 3;
    private int playerCurrentHealth;

    public RoomObject  ActualRoom;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            facade = null;
        }

        //DontDestroyOnLoad(gameObject);

        playerCurrentHealth = playerMaxHealth;
        //if (Instance == this)
        //{
        //    facade.StartGame();
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        playerCurrentHealth = playerMaxHealth;
        iterations = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetPlayerCurrentHealth()
    {
        return playerCurrentHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }

    public void TakePlayerDamage(int damage)
    {
        playerCurrentHealth -= damage;
        playerCurrentHealth = Mathf.Max(playerCurrentHealth, 0);
    }

    public void ResetPlayerHealth()
    {
        playerCurrentHealth = playerMaxHealth;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
