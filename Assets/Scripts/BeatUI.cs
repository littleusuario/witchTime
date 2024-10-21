using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class BeatUI : MonoBehaviour
{
    [SerializeField] private float stepDistance = 0.1f;
    private float speed;
    [SerializeField] float thresholdDissapear;
    public float Speed { get => speed; set => speed = value; }

    private ObjectPool<BeatUI> beatPool;
    public ObjectPool<BeatUI> BeatPool { get => beatPool; set => beatPool = value; }

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 newPos = transform.position + Vector3.right * speed * Time.deltaTime;
        newPos.y = transform.position.y;
        newPos.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, newPos, stepDistance);

        if (rectTransform.anchoredPosition.x > -thresholdDissapear && rectTransform.anchoredPosition.x < thresholdDissapear) 
        {
            beatPool.Release(this);
        }
    }
}
