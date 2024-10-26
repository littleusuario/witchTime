using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletData", menuName = "Flyweight/BulletData", order = 1)]
public class BulletScriptable : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public int damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
