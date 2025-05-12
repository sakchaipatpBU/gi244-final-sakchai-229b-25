using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public GameObject gameOverScreen;


    public RectTransform hpFill;
    public float maxWidth = 200f;

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        gameOverScreen.SetActive(false);
    }

    


    private void Update()
    {
        scoreText.text = Score.GetInstance().score.ToString();
    }

    [System.Serializable]
    class SaveData
    {
        public float score;
        public float highScore;
    }

    public void SaveScore(float score)
    {
        Debug.Log(Application.persistentDataPath);
        string fileName = "save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;

        float highScore = LoadHighScore();
        Debug.Log("high "+highScore);

        SaveData data = new SaveData();
        data.score = score;
        data.highScore = highScore;

        // บันทึก high score ใหม่
        if (score > highScore)
        {
            data.highScore = score;
        }

        string content = JsonUtility.ToJson(data);

        File.WriteAllText(filePath, content);

        highScoreText.text = "High Score : "+data.highScore.ToString();
    }

    public float LoadHighScore()
    {
        string fileName = "save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;

        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);

            // ถ้า content ว่างหรือไม่ใช่ JSON
            if (string.IsNullOrEmpty(content))
            {
                Debug.Log("Save empty");
                return 0f;
            }

            SaveData data = JsonUtility.FromJson<SaveData>(content);

            // เช็คว่ามันแปลงแล้วเป็น null ไหม
            if (data == null)
            {
                Debug.Log("Failed to parse save data");
                return 0f;
            }

            return data.highScore;
        }
        else
        {
            Debug.Log("Save file not found");
            return 0f;
        }

    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void UpdateHPBar(int currentHP, int maxHP)
    {
        float ratio = (float)currentHP / maxHP;
        float width = ratio * maxWidth;
        hpFill.sizeDelta = new Vector2(width, hpFill.sizeDelta.y);
    }

}
