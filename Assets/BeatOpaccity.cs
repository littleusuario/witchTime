using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatOpaccity : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] RawImage beatSimbol;
    private int distance = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     Vector3 distancePlayer = (new Vector3 (player.transform.position.x,0,player.transform.position.z));
     Vector3 distanceUi = (new Vector3(beatSimbol.transform.position.x, 0, beatSimbol.transform.position.z));

      float distanceBetween = Vector3.Distance(distanceUi,distancePlayer);

        Debug.Log(distanceBetween);
       
        beatSimbol.GetComponent<RawImage>().color =  new Color (1,1,1,distanceBetween);
        
    }
}
