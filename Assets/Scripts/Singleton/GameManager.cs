using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;  // Asegúrate de incluir este namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGamePaused;

    [Header("Player Health")]
    public int playerMaxHealth = 4;
    public int playerCurrentHealth;

    [SerializeField] private FacadeManager facade;
    public SpawnEnemies spawnEnemies;
    public int iterations = 0;
    public Room_Normal ActualRoom;
    public bool BeatUIHelpActive = true;
    public bool SoundHelpActive = false;

    [Header("Cinemachine Camera Shake")]
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    [Header("Main Camera Volume")]
    public Camera mainCamera;
    private Volume cameraVolume;
    public int HighScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        playerCurrentHealth = playerMaxHealth;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += GetLevelReferences;
    }

    private void GetLevelReferences(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject FacadeObject = GameObject.FindGameObjectWithTag("Facade");

        if (FacadeObject != null)
        {
            facade = FacadeObject.GetComponent<FacadeManager>();
            spawnEnemies = FacadeObject.GetComponent<SpawnEnemies>();
            facade.StartGame();
        }

        GameObject BeatUIHelpObject = GameObject.FindGameObjectWithTag("BeatVisualHelp");

        if (BeatUIHelpObject != null)
        {
            BeatUIHelpObject.SetActive(BeatUIHelpActive ? true : false);
        }

        GameObject SoundHelpObject = GameObject.FindGameObjectWithTag("SoundHelp");

        if (SoundHelpObject != null)
        {
            SoundHelpObject.SetActive(SoundHelpActive ? true : false);
        }

        SetupCinemachine();

    }

    private void SetupCinemachine()
    {
        GameObject cinemachineObject = GameObject.FindGameObjectWithTag("Cinemachine");

        if (cinemachineObject != null)
        {
            virtualCamera = cinemachineObject.GetComponent<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (perlin != null)
                {
                    perlin.m_AmplitudeGain = 0f;
                    perlin.m_FrequencyGain = 0f;
                }
            }
        }
    }

    private void SetupCamera() 
    {
        mainCamera = Camera.main;

        if (mainCamera != null) 
        {
            cameraVolume = mainCamera.GetComponent<Volume>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void RestartGame()
    {
        playerCurrentHealth = playerMaxHealth;
        iterations = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PauseGame()
    {
        playerCurrentHealth = playerMaxHealth;
        iterations = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public int GetPlayerCurrentHealth()
    {
        return playerCurrentHealth;
    }

    public void SetCurrentRoom(RoomObject roomObject)
    {
        if (Instance.ActualRoom != null)
        {
            Instance.ActualRoom.IsCurrentRoom(false);
        }

        Instance.ActualRoom = (Room_Normal)roomObject;
        Instance.ActualRoom.IsCurrentRoom(true);
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
        iterations = 0;
        playerCurrentHealth = playerMaxHealth;
    }

    public void LoadNextLevel()
    {
        iterations++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGameForSeconds(float seconds)
    {
        if (!IsGamePaused)
        {
            StartCoroutine(PauseCoroutine(seconds));
        }
    }


    public void TriggerCameraShake(float amplitude, float frequency, float duration)
    {
        if (perlin != null)
        {
            StartCoroutine(CameraShakeCoroutine(amplitude, frequency, duration));
        }
    }
    public void InitializeDeathSequence() 
    {
        StartCoroutine(DeathCoroutine(0.5f));
    }

    IEnumerator DeathCoroutine(float seconds) 
    {
        SceneManager.LoadScene("GameOverScreen");
        yield return new WaitForSeconds(seconds);
        playerCurrentHealth = playerMaxHealth;
    }
    private IEnumerator PauseCoroutine(float seconds)
    {
        IsGamePaused = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(seconds);

        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    private IEnumerator CameraShakeCoroutine(float targetAmplitude, float targetFrequency, float duration)
    {
        int steps = 30;
        float stepDuration = duration / steps;

        perlin.m_AmplitudeGain += targetAmplitude;
        perlin.m_FrequencyGain += targetFrequency;

        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSeconds(stepDuration);
        }

        perlin.m_AmplitudeGain = Mathf.Max(0f, perlin.m_AmplitudeGain - targetAmplitude);
        perlin.m_FrequencyGain = Mathf.Max(0f, perlin.m_FrequencyGain - targetFrequency);
    }

}
