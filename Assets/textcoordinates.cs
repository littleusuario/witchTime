using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textcoordinates : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        textMeshProUGUI.text = mousePOS.ToString();
    }
}
