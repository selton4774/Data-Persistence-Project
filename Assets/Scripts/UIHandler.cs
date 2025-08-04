using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI NameText;

    public void SetPlayersName()
    {
        if(NameText != null)
            GameMananger.Instance.Name = NameText.text;    
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
        SceneManager.LoadScene(1);
    }
}
