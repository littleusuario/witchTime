using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    public TextMeshProUGUI pisoText;
    public GameObject pisoObject;
    public AudioClip transitionSound;
    public ParticleSystem particleSystem1;
    public ParticleSystem particleSystem2;

    private Animator pisoAnimator;
    private AudioSource audioSource;
    private int currentIteration;

    void Start()
    {
        pisoAnimator = pisoObject.GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = transitionSound;

        StartCoroutine(PisoTransition());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }

    IEnumerator PisoTransition()
    {
        currentIteration = GameManager.Instance.iterations;
        pisoText.text = "Piso " + currentIteration;

        yield return new WaitForSeconds(3);

        pisoText.text = "Piso " + (currentIteration + 1);

        audioSource.Play();
        pisoAnimator.SetBool("isAnimating", true);

        yield return new WaitForSeconds(2.25f);

        particleSystem1.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        particleSystem2.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        yield return new WaitForSeconds(2.25f);

        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (currentIteration < 3) 
        {
            SceneManager.LoadScene("EasyGeneration");
        } 
        else if (currentIteration < 6) 
        {
            SceneManager.LoadScene("MediumGeneration");
        }
        else if (currentIteration < 999)
        {
            SceneManager.LoadScene("HardGeneration");
        }
    }
}
