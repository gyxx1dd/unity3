using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene(1); 
    }

    public void SettingsButton()
    {
        Debug.Log("Settings clicked");
    }

    public void ExitButton()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
    #else
        Application.Quit(); 
    #endif
        Debug.Log("Exit clicked");
    }
}
