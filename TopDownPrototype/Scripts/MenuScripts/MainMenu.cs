using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public GameObject SettingsCanvas;

    private void Awake()
    {
        MainMenuCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoToSettings()
    {
        if (MainMenuCanvas.activeInHierarchy)
        {
            MainMenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
        }
        else if (SettingsCanvas.activeInHierarchy)
        {
            SettingsCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
        }
    }
}
