using UnityEngine;

public class FacadeManager : MonoBehaviour
{
    [SerializeField] LevelGenerator levelGenerator;
    public void StartGame()
    {
        levelGenerator.CreateLevelProcess();
    }
}
