using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    [SerializeField] private BulletSpawner rotation;
    
    private ObjectPool<Bullet> bullpool;
    
    public ObjectPool<Bullet> BullPool { get { return bullpool; } set { bullpool = value; } }
    public static BulletPool instance;
    public Bullet prefab;

    void Awake()
    {
        bullpool = new ObjectPool<Bullet>(CreateBullet, Ontake, OnReturned, OnDestroyed, false, 10, 100);
    }

    private void OnDestroyed(Bullet Bullet)
    {
        Destroy(Bullet.gameObject);
    }

    private void OnReturned(Bullet Bullet)
    {
        Bullet.gameObject.SetActive(false);
    }

    private void Ontake(Bullet Bullet)
    {
      Bullet.gameObject.SetActive(true);
    }

    private Bullet CreateBullet()
    {
      Bullet tempbull = Instantiate(prefab, spawn.transform.position, Quaternion.Euler(90, 0, 0));
        tempbull.gameObject.SetActive(false);
        tempbull.Pool = bullpool;
      return tempbull;

    }
}
