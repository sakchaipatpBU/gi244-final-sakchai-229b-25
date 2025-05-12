using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject powerupPrefab;

    public float startDelay = 2;
    public float repeatRate = 10;

    private PlayerController playerController;

    
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
            Instantiate(powerupPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
