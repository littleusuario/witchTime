using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class BeatUI : MonoBehaviour
{
    [SerializeField] private float thresholdDissapear = 10f;
    private ObjectPool<BeatUI> beatPool;
    private RectTransform rectTransform;
    private RawImage rawImage;
    private float currentOpacity;

    public ObjectPool<BeatUI> BeatPool
    {
        get => beatPool;
        set => beatPool = value;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();
    }

    public void ResetOpacity()
    {
        currentOpacity = 0f;
        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = currentOpacity;
            rawImage.color = color;
        }
    }

    private void Update()
    {
        if (currentOpacity < 1f)
        {
            currentOpacity = Mathf.Lerp(currentOpacity, 1f, 0.05f);
            currentOpacity = Mathf.Clamp(currentOpacity, 0f, 1f);

            if (rawImage != null)
            {
                Color color = rawImage.color;
                color.a = currentOpacity;
                rawImage.color = color;
            }
        }

        if (Mathf.Abs(rectTransform.anchoredPosition.x) < thresholdDissapear)
        {
            beatPool.Release(this);
        }
    }

    public IEnumerator LerpToPosition(float duration)
    {
        float startTime = Time.realtimeSinceStartup;
        Vector3 initialPosition = rectTransform.anchoredPosition;
        Vector3 endPosition = new Vector3(0, rectTransform.anchoredPosition.y);

        while (Time.realtimeSinceStartup - startTime < duration)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, endPosition, elapsed / duration);
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
        beatPool.Release(this);
    }
}
