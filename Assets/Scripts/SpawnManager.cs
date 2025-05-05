using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] obstaclePrefab;

    public float startDelay = 2;
    public float repeatRate = 2;

    private PlayerController playerController;

    private static SpawnManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static SpawnManager GetInstance()
    {
        return instance;
    }
    void Start()
    {
        InvokeRepeating(nameof(Spawn), startDelay, repeatRate);
        var go = GameObject.Find("Player");
        playerController = go.GetComponent<PlayerController>();
    }

    void Spawn()
    {
        if (playerController.isGameOver == false)
        {
            int indexRandom = Random.Range(0, obstaclePrefab.Length);
            var obstacle = ObstacleObjectPool.GetInstance().Acquire(indexRandom);
            obstacle.transform.SetPositionAndRotation(spawnPoint.position, gameObject.transform.rotation);
        }
    }
}
