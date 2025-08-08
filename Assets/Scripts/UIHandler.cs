using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIHandler : MonoBehaviour
{
    public TMP_InputField NameText;
    public TextMeshProUGUI ErroMessage;

    public void SetPlayersName()
    {
        if (NameText != null)
        {
            GameMananger.Instance.Name = NameText.text; 
        }

        LoadData();
    }


    public static PlayerData? GetPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "data.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            return new PlayerData
            {
                name = data.name,
                score = data.score
            };  
        }

        return null;
    }

    private void LoadData()
    {
        var data = GetPlayerData();

        if (data != null)
        {
            GameMananger.Instance.HighScorePlayerName = data.name;
            GameMananger.Instance.HighScore = data.score;

            Debug.Log("carregou os dados.");

        } else
        {
            Debug.Log("erro ao carregar os dados.");
        }
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(NameText.text))
        {
            ErroMessage.text = "Você precisa fornecer o seu nome.";
            ErroMessage.gameObject.SetActive(true);

            StartCoroutine(HideErrorMessageAfterDelay(5.0f));
        } else
        {
            SceneManager.LoadScene(1);
        }
    }

    private System.Collections.IEnumerator HideErrorMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ErroMessage.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int score;
    }

}
