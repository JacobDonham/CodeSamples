using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class New_Continue_Game : MonoBehaviour
{
    public Button continueButton;

    private void Awake()
    {
        string path = Application.persistentDataPath + "/player.TBA";

        if (File.Exists(path))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    public void NewGame()
    {
        SaveSystem.NewGame();
        SceneManager.LoadScene(2);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(2);
        /*
        if (File.Exists(path))
        {
            
        }
        else
        {
            continueButton.interactable = false;
        }*/
    }
}
