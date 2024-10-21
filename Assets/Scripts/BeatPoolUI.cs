using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        beatPool = new ObjectPool<BeatUI>(createItem, GetItemFromPool, OnReturnItemFromPool, OnDestroyItemFromPool, checkCollection, defaultCapacity, maxCapacity);
    }

    public void CallPool() 
    {
        BeatUI beat = beatPool.Get();
        beat.transform.position = transform.position;
    }
    public BeatUI createItem() 
    {
        BeatUI beat = Instantiate(beatUIPrefab, beatParent);
        beat.BeatPool = beatPool;
        beat.Speed = speedOfBeat;
        beat.gameObject.SetActive(false);
        return beat;
    }

    public void GetItemFromPool(BeatUI beat) 
    {
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
