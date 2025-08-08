using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UIHandler;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text NewRecordScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        if (!string.IsNullOrEmpty(GameMananger.Instance.HighScorePlayerName))
            SetMaxScore(true);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Salvou os dados.");
        SaveData();
    }


    private void WriteFile()
    {
        PlayerData data = new PlayerData
        {
            name = GameMananger.Instance.Name,
            score = GameMananger.Instance.Score
        };

        string json = JsonUtility.ToJson(data);

        string path = Path.Combine(Application.persistentDataPath, "data.json");

        File.WriteAllText(path, json);
    }

    private void SaveData()
    {
        var player = UIHandler.GetPlayerData();

        if (player == null)
            WriteFile();
        else if (player != null && GameMananger.Instance.Score > player.score)
            WriteFile();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }


    void SetMaxScore(bool isNewGame)
    {
        if (isNewGame)
        {
            NewRecordScoreText.text = $"Best Score : {GameMananger.Instance.HighScorePlayerName} : {GameMananger.Instance.HighScore}";
        } else
        {
            GameMananger.Instance.Score = m_Points;

            GameMananger.Instance.HighScore = m_Points;
            GameMananger.Instance.HighScorePlayerName = GameMananger.Instance.Name;

            NewRecordScoreText.text = $"Best Score : {GameMananger.Instance.Name} : {GameMananger.Instance.Score}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > GameMananger.Instance.HighScore)
            SetMaxScore(false);   
    }
}
