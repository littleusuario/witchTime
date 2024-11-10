using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI HighScore;
    public GameObject GameOverRoom;
    public Vector3 endPosition = Vector3.zero;
    public float TimeToEndTransition = 10f;
    public GameObject Cam;
    public GameObject RecordObject;
    private NoiseMovement noiseMovement;
    public ParticleSystem ParticleSystem;

    public List<GameObject> UIToAnimate;
    public float DelayTime = 0.3f;
    void Start()
    {
        if (HighScore != null)
            HighScore.text =  $"{(GameManager.Instance.iterations + 1).ToString()}";

        if (GameManager.Instance.iterations > GameManager.Instance.HighScore) 
        {
            RecordObject.SetActive(true);
            GameManager.Instance.HighScore = GameManager.Instance.iterations;
            GameManager.Instance.ResetPlayerHealth();
        }
        Cam = Camera.main.gameObject;
        noiseMovement = Cam.GetComponent<NoiseMovement>();
        StartCoroutine(GameOverTransition());
    }

    IEnumerator GameOverTransition() 
    {
        Vector3 initialPosition = Cam.transform.position;
        float elapsedTime = 0;
        noiseMovement.enabled = false;

        while (elapsedTime < TimeToEndTransition) 
        {
            elapsedTime += Time.deltaTime;
            Cam.transform.position = Vector3.Lerp(initialPosition, endPosition, elapsedTime / TimeToEndTransition);

            if( elapsedTime > TimeToEndTransition / 2) 
            {
                ParticleSystem.gameObject.SetActive(false);
            }
            yield return null;
        }
        Cam.transform.position = endPosition;
        noiseMovement.StartPos = endPosition;
        yield return new WaitForSeconds(0.3f);
        noiseMovement.enabled = true;
        GameOverRoom.SetActive(true);

        foreach (GameObject item in UIToAnimate)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(DelayTime);
        }
    }
}
