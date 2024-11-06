using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private BulletPool bulletpool;
    [SerializeField] private GameObject spawn;
    [SerializeField] private Enemy subjecttoObserve;
    [SerializeField] private BeatManager beatManager;

    private Ishoot shoot;
    private Straightshoot straightShoot;
    private Diagonalshoot diagonalShoot;

    private bool shouldShoot = false;

    private void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("BeatManager");
        beatManager = gameObject.GetComponent<BeatManager>();
        beatManager.Intervals[0]._Trigger.AddListener(Shooting);

        shoot = GetComponent<Ishoot>();
        straightShoot = GetComponent<Straightshoot>();
        diagonalShoot = GetComponent<Diagonalshoot>();

        shoot = diagonalShoot;

        if (subjecttoObserve != null)
        {
            subjecttoObserve.Ondie += Ondie;
        }
    }

    public void Shooting()
    {
        if (shouldShoot)
        {
            setBulletType();

            shoot.shoot(bulletpool, spawn);
            shouldShoot = !shouldShoot;
        }
        else
        {
            shouldShoot = !shouldShoot;
        }
    }

    private void setBulletType()
    {
        if (shoot == diagonalShoot)
        {
            shoot = straightShoot;
        }
        else if (shoot == straightShoot)
        {
            shoot = diagonalShoot;
        }
    }

    private void Ondie()
    {
        Destroy(this);
    }
}
