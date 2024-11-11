using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class BeatPoolUI : MonoBehaviour
{
    [SerializeField] private float bpm = 90f;
    [SerializeField] private BeatUI beatUIPrefab;
    [SerializeField] private bool checkCollection = false;
    [SerializeField] private int defaultCapacity = 2;
    [SerializeField] private int maxCapacity = 2;
    [SerializeField] private Transform beatParent;

    private ObjectPool<BeatUI> beatPool;
    private float shootingTime;
    private float lastBeatTime;

    private void Start()
    {
        shootingTime = 60f / bpm;

        beatPool = new ObjectPool<BeatUI>(CreateItem, GetItemFromPool, OnReturnItemFromPool, OnDestroyItemFromPool, checkCollection, defaultCapacity, maxCapacity);

        lastBeatTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        float currentTime = Time.realtimeSinceStartup;

        if (currentTime - lastBeatTime >= shootingTime)
        {
            lastBeatTime = currentTime;
            BeatUI beat = beatPool.Get();
            beat.transform.position = transform.position; 
            beat.StartCoroutine(beat.LerpToPosition(shootingTime));
        }
    }

    private BeatUI CreateItem()
    {
        BeatUI beat = Instantiate(beatUIPrefab, beatParent);
        beat.BeatPool = beatPool;
        beat.gameObject.SetActive(false);
        beat.ResetOpacity();
        return beat;
    }

    private void GetItemFromPool(BeatUI beat)
    {
        beat.ResetOpacity();
        beat.gameObject.SetActive(true);
    }

    private void OnReturnItemFromPool(BeatUI beat)
    {
        beat.gameObject.SetActive(false);
    }

    private void OnDestroyItemFromPool(BeatUI beat)
    {
        Destroy(beat.gameObject);
    }
}
