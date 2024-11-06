using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FacadeManager : MonoBehaviour
{
    [SerializeField] LevelGenerator levelGenerator;
    public void StartGame()
    {
        levelGenerator.CreateLevelProcess();
    }
}
