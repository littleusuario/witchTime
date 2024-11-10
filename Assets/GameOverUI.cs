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
    private NoiseMovement noiseMovement;
    void Start()
    {
        if (HighScore != null)
            HighScore.text =  $"{(GameManager.Instance.iterations + 1).ToString()}";
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
            yield return null;
        }
        Cam.transform.position = endPosition;
        noiseMovement.StartPos = endPosition;
        yield return new WaitForSeconds(0.3f);
        noiseMovement.enabled = true;
        GameOverRoom.SetActive(true);
    }
}
