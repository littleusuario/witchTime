using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{

    public float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<Intervals> _intervals = new List<Intervals>();
    [SerializeField] private bool _beatDetected;
    public List<Intervals> Intervals { get => _intervals; set => _intervals = value; }

    private void Update()
    {
        foreach (Intervals interval in _intervals)
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }

    public void SetBeatDetected(bool value)
    {
        _beatDetected = value;
    }
}

[System.Serializable]
public class Intervals
{

    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastInterval;

    public UnityEvent _Trigger { get => _trigger; set => _trigger = value; }

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
