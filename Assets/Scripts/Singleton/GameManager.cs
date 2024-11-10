using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Player Health")]
    public int playerMaxHealth = 4;
    
    [SerializeField] private FacadeManager facade;
    
    public int playerCurrentHealth;
    
    public SpawnEnemies spawnEnemies;
    public int iterations = 0;
    public Room_Normal ActualRoom;
    public bool BeatUIHelpActive = true;
    public bool SoundHelpActive = false;

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
        Instance.ActualRoom = (Room_Normal)roomObject;
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
        iterations++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
