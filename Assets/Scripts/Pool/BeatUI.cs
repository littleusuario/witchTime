using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class BeatUI : MonoBehaviour
{
    [SerializeField] private float stepDistance = 0.1f;
    [SerializeField] private float thresholdDissapear;
    private float speed;

    private ObjectPool<BeatUI> beatPool;
    public ObjectPool<BeatUI> BeatPool { get => beatPool; set => beatPool = value; }

    private RectTransform rectTransform;
    private RawImage rawImage;
    public float currentOpacity = 0f;

    private BeatManager beatManager;

    public float Speed { get => speed; set => speed = value; }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();
        Color color = rawImage.color;
        color.a = currentOpacity;
        rawImage.color = color;

        GameObject beatManagerObject = GameObject.FindGameObjectWithTag("BeatManager");
        if (beatManagerObject != null)
        {
            beatManager = beatManagerObject.GetComponent<BeatManager>();
            float secondsPerBeat = 60f / beatManager._bpm;
            speed = secondsPerBeat;
        }
    }

    void Update()
    {
        if (currentOpacity < 1f)
        {
            currentOpacity = Mathf.Lerp(currentOpacity, 1f, Time.deltaTime * 2f);
            currentOpacity = Mathf.Clamp(currentOpacity, 0f, 1f);

            Color color = rawImage.color;
            color.a = currentOpacity;
            rawImage.color = color;
        }

        if (rectTransform.anchoredPosition.x > -thresholdDissapear && rectTransform.anchoredPosition.x < thresholdDissapear)
        {
            beatPool.Release(this);
        }
    }

    public IEnumerator LerpToPosition()
    {
        rectTransform = GetComponent<RectTransform>();
        float elapsedTime = 0;
        Vector3 initialPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = new Vector3(0, rectTransform.anchoredPosition.y);

        float duration = speed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, endPosition, elapsedTime / duration);
            yield return null;
        }
        rectTransform.anchoredPosition = endPosition;
        beatPool.Release(this);
    }
}
