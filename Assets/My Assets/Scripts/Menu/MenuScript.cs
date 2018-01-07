using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject InstructionsPanel;
    [SerializeField] GameObject CreditsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowInstructions()
    {
        MainMenuPanel.SetActive(false);
        InstructionsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        InstructionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }



}
