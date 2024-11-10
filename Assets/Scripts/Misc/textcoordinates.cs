using TMPro;
using UnityEngine;

public class textcoordinates : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        Vector3 mouseToWorldScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        textMeshProUGUI.text = $"Posición mouse en mundo: {mouseToWorldScreen} \n ´Posición jugador: {player.transform.position} \n {-AngleBetweenTwoPoints(player.transform.position, mouseToWorldScreen)}";
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
