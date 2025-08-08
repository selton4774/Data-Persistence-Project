using System.Collections;
using UnityEngine;

public class GameMananger : MonoBehaviour
{
    public int Score;
    public string Name;

    public int HighScore;
    public string HighScorePlayerName;

    public static GameMananger Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
