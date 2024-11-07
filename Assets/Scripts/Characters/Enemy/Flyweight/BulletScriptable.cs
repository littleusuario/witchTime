using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletData", menuName = "Flyweight/BulletData", order = 1)]
public class BulletScriptable : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public int damage;
}
