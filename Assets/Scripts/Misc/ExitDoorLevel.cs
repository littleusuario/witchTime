using System.Collections;
using UnityEngine;

public class ExitDoorLevel : MonoBehaviour
{
    Animator animator;
    float elapsedTime = 0;
    bool doTransitionOnce;
    GameObject player;
    bool cameraShake = false;

    public float TimeToStartTranstion = 1.7f;
    public GameObject CameraFollowObject;

    public AudioClip doorSound;
    private AudioSource audioSource;
    private AudioSource beatManagerAudioSource;

    private FadeController fadeController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        CameraFollowObject = GameObject.FindGameObjectWithTag("ObjectFollowCam");
        player = GameObject.FindGameObjectWithTag("Player");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameObject beatManager = GameObject.FindGameObjectWithTag("BeatManager");
        if (beatManager != null)
        {
            beatManagerAudioSource = beatManager.GetComponent<AudioSource>();
        }

        GameObject fadeObject = GameObject.FindGameObjectWithTag("FadeController");
        if (fadeObject != null)
        {
            fadeController = fadeObject.GetComponent<FadeController>();
        }
    }

    public void InitializeTransition()
    {
        animator.SetBool("Closing", true);
        player.SetActive(false);
        GameManager.Instance.IsExitTransitionActive = true;
        doTransitionOnce = true;
        elapsedTime += Time.deltaTime;

        if (doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }

        if (beatManagerAudioSource != null)
        {
            StartCoroutine(ReduceVolume(beatManagerAudioSource, 0.75f, 0f, 2f));
        }
    }

    void Update()
    {
        if (doTransitionOnce)
        {
            if (!cameraShake)
            {
                GameManager.Instance.TriggerCameraShake(0.25f, 3f, 1f);
                cameraShake = true;
            }

            if (elapsedTime >= TimeToStartTranstion)
            {
                StartCoroutine(MoveCamera());
                doTransitionOnce = false;
            }
            else
                elapsedTime += Time.deltaTime;
        }
    }

    IEnumerator MoveCamera()
    {
        if (fadeController != null)
        {
            StartCoroutine(fadeController.FadeIn(-0.25f));
        }

        float elapsedTime = 0;
        Vector3 initialPosition = CameraFollowObject.transform.position;
        Vector3 endPosition = new Vector3(initialPosition.x, -15, initialPosition.z);

        while (elapsedTime < TimeToStartTranstion)
        {
            elapsedTime += Time.deltaTime;
            CameraFollowObject.transform.position = Vector3.Lerp(initialPosition, endPosition, elapsedTime / TimeToStartTranstion);
            yield return null;
        }

        CameraFollowObject.transform.position = endPosition;

        GameManager.Instance.LoadNextLevel();
    }

    IEnumerator ReduceVolume(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float elapsedTime = 0f;
        audioSource.volume = startVolume;

        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = endVolume;
    }
}
