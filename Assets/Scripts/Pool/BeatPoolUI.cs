using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class BeatPoolUI : MonoBehaviour
{
    [SerializeField] float speedOfBeat;
    [SerializeField] BeatUI beatUIPrefab;
    [SerializeField] bool checkCollection = false;
    [SerializeField] int defaultCapacity = 2;
    [SerializeField] int maxCapacity = 2;
    [SerializeField] Transform beatParent;
    ObjectPool<BeatUI> beatPool;
    bool once;
    float elapsedTime;
    [SerializeField] float shootingTime = 1;

    void Start()
    {
        beatPool = new ObjectPool<BeatUI>(createItem, GetItemFromPool, OnReturnItemFromPool, OnDestroyItemFromPool, checkCollection, defaultCapacity, maxCapacity);
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > shootingTime)
        {
            elapsedTime = 0;
            BeatUI beat = beatPool.Get();
            beat.transform.position = transform.position;
            beat.StartCoroutine(beat.LerpToPosition());
        }
    }

    public BeatUI createItem()
    {
        BeatUI beat = Instantiate(beatUIPrefab, beatParent);
        beat.BeatPool = beatPool;
        beat.Speed = speedOfBeat;
        beat.gameObject.SetActive(false);

        beat.currentOpacity = 0f;
        RawImage rawImage = beat.GetComponent<RawImage>();
        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = beat.currentOpacity;
            rawImage.color = color;
        }

        return beat;
    }


    public void GetItemFromPool(BeatUI beat)
    {
        beat.currentOpacity = 0f;
        RawImage rawImage = beat.GetComponent<RawImage>();
        if (rawImage != null)
        {
            Color color = rawImage.color;
            color.a = beat.currentOpacity;
            rawImage.color = color;
        }

        beat.gameObject.SetActive(true);
    }

    public void OnReturnItemFromPool(BeatUI beat)
    {
        beat.gameObject.SetActive(false);
    }

    public void OnDestroyItemFromPool(BeatUI beat)
    {
        Destroy(beat.gameObject);
    }
}