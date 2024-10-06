using UnityEngine;

public class NoiseMovement : MonoBehaviour
{
    public float speed = 1f;
    public float scale = 1f;
    public int seed = 0;
    private Vector3 startPos;
    private float seedOffsetX;
    private float seedOffsetY;

    void Start()
    {
        startPos = transform.position;
        seedOffsetX = Random.Range(-1000f, 1000f) + seed;
        seedOffsetY = Random.Range(-1000f, 1000f) + seed;
    }

    void Update()
    {
        float time = Time.time * speed;

        float noiseX = Mathf.PerlinNoise(seedOffsetX + time, 0.0f) * 2 - 1;
        float noiseY = Mathf.PerlinNoise(0.0f, seedOffsetY + time) * 2 - 1;
        float noiseZ = Mathf.PerlinNoise(seedOffsetX + time, seedOffsetY + time) * 2 - 1;

        Vector3 newPos = new Vector3(noiseX, noiseY, noiseZ) * scale + startPos;

        transform.position = newPos;
    }
}
