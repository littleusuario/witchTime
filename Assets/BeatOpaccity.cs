using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatOpaccity : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] RawImage beatSimbol;
    void Update()
    {
        Vector3 distancePlayer = new Vector3 (player.transform.position.x, 0, player.transform.position.z);
        Vector3 distanceUi = new Vector3 (beatSimbol.transform.position.x, 0, beatSimbol.transform.position.z - 1.1f);

        float distanceBetween = Vector3.Distance(distanceUi,distancePlayer);

        Debug.Log(distanceBetween);
       
        if (distanceBetween >= 0)
            beatSimbol.GetComponent<RawImage>().color =  new Color (1,1,1,distanceBetween);
     
    }
}
