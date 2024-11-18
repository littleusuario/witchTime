using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseToTheBeat : MonoBehaviour
{
    [SerializeField] bool _useTestBeat;
    [SerializeField] float _pulseSize = 1.15f;
    [SerializeField] float _returnSpeed = 5f;
    [SerializeField] GameObject attackObject;
    [SerializeField] List<GameObject> redBeats = new List<GameObject>();
    [SerializeField] GameObject beatCanvas;

    private Vector3 _startSize;
    private Vector3 _startPosition;
    private bool pulseInput;
    private bool _isClick;
    private bool _hasAttacked;
    private float startY;

    public bool _isPulsing;
    public event Action beatPulse;
    public bool PulseInput => pulseInput;
    public float PulseSize { get => _pulseSize; set => _pulseSize = value; }
    private BeatManager BeatManager;

    private void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("BeatManager");
        BeatManager = gameObject.GetComponent<BeatManager>();
        BeatManager.Intervals[0]._Trigger.AddListener(Pulse);

        _startSize = transform.localScale;
        _startPosition = transform.position;
        startY = _startPosition.y;

        if (_useTestBeat)
        {
            StartCoroutine(TestBeat());
        }

        if (attackObject != null)
        {
            attackObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (beatCanvas != null) 
        {
            foreach (Transform child in beatCanvas.transform) 
            {
                if (!redBeats.Contains(child.gameObject)) 
                {
                    redBeats.Add(child.gameObject);
                }
            }
        }
        if (Input.GetMouseButton(0) && attackObject != null && !_isClick) 
        {
            pulseInput = true;
            foreach (GameObject beat in redBeats) 
            {
                beat.gameObject.GetComponent<RawImage>().color = Color.red;
            }
        }

        if (_isPulsing)
        {
            Vector3 newScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
            transform.localScale = new Vector3(newScale.x, newScale.y, _startSize.z);

            if (transform.localScale.magnitude < (_startSize * _pulseSize).magnitude && !_hasAttacked)
            {
                if(attackObject != null && pulseInput)
                {
                    Attack();
                    pulseInput = false;
                }
            }

            if (transform.localScale == _startSize)
            {
                _isPulsing = false;
                _hasAttacked = false;
            }
        }
    }

    public void Pulse()
    {
        _isPulsing = true;
        _hasAttacked = false;

        if (beatPulse != null) 
        {
            beatPulse.Invoke();
        }

        if (beatCanvas != null)
        {
            foreach (GameObject beat in redBeats)
            {
                beat.gameObject.GetComponent<RawImage>().color = Color.white;
            }
        }

        transform.localScale = _startSize * _pulseSize;
    }

    public void Attack()
    {
        _isClick = true;
        attackObject.SetActive(true);
        _hasAttacked = true;
        _isClick = false;

        StartCoroutine(DisableAttackObject());
    }

    IEnumerator DisableAttackObject()
    {
        yield return new WaitForSeconds(0.1f);
        attackObject.SetActive(false);
    }

    IEnumerator TestBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Pulse();
        }
    }
}
