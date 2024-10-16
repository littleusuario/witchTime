using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PulseToTheBeat : MonoBehaviour
{
    [SerializeField] bool _useTestBeat;
    [SerializeField] float _pulseSize = 1.15f;
    [SerializeField] float _returnSpeed = 5f;
    [SerializeField] GameObject attackObject;

    private Vector3 _startSize;
    private Vector3 _startPosition;
    public bool _isPulsing;
    private bool _isClick;
    private bool _hasAttacked;
    private float startY;

    public event Action beatPulse;

    private bool pulseInput;
    public bool PulseInput => pulseInput;
    [SerializeField] RawImage Image;

    private void Start()
    {
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
        if (Input.GetMouseButton(0) && attackObject != null && !_isClick) 
        {
            pulseInput = true;
        }

        if (Image != null) 
        {
            Image.color = pulseInput ? Color.red : Color.white;
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
