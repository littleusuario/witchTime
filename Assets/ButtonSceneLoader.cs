using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneLoader : MonoBehaviour
{
    [SerializeField] private string nameOfLoadLevel;
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null) 
        {
            button.onClick.AddListener(LoadLevel);
        }
    }
    public void LoadLevel() 
    {
        SceneManager.LoadScene(nameOfLoadLevel);
    }
}
