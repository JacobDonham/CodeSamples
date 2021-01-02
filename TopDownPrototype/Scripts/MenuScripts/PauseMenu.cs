using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject SettingsCanvas;
    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        PauseCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else if (!isPaused)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void SettingsMenu()
    {
        Time.timeScale = 0;

        if (PauseCanvas.activeInHierarchy)
        {
            PauseCanvas.SetActive(false);
            SettingsCanvas.SetActive(true);
        }
        else if (SettingsCanvas.activeInHierarchy)
        {
            SettingsCanvas.SetActive(false);
            PauseCanvas.SetActive(true);
        }
    }
}
