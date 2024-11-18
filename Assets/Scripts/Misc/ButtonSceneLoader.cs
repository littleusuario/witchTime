using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonSceneLoader : MonoBehaviour
{
    [SerializeField] private string nameOfLoadLevel;
    [SerializeField] private AudioSource StartGame;
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
        StartCoroutine(Playsound());
        
    }
    IEnumerator Playsound()
    {  
        StartGame.Play();
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(nameOfLoadLevel);

    }
}
