using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{

    public float score;
    public float scoreMultiplier = 1f;

    private bool isRunning = true;

    private static Score instance;
    public static Score GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (isRunning)
        {
            score += Time.deltaTime * scoreMultiplier;
        }
    }

    public void StopScore()
    {
        isRunning = false;
    }

    public void ResetScore()
    {
        score = 0;
        isRunning = true;
    }
}
