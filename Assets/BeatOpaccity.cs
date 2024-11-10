using UnityEngine;
using UnityEngine.UI;

public class BeatOpaccity : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] RawImage beatSymbol;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        beatSymbol = GetComponent<RawImage>();
    }
    void Update()
    {

        if (player.transform.position.z < GameManager.Instance.ActualRoom.transform.position.z) 
        {
            beatSymbol.color = Color.Lerp(beatSymbol.color, new Color(beatSymbol.color.r, beatSymbol.color.g, beatSymbol.color.b, 0.5f), Time.deltaTime * 5f);
        }
        else 
        {
            beatSymbol.color = Color.Lerp(beatSymbol.color, new Color(beatSymbol.color.r, beatSymbol.color.g, beatSymbol.color.b, 1f), Time.deltaTime * 5f);
        }
    }
}
