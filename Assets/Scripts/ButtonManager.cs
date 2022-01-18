using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("Trench1");
    }

    public void btn_Quitgame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void btn_Tutorial()
    {
        SceneManager.LoadScene("ControlMenu");
    }

    public void btn_Credits()
    {
        SceneManager.LoadScene("CreditMenu");
    }

    public void btn_MainMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }

    public void btn_NextLevel()
    {
        if (LevelManager.S.finalScene)
        {
            GameManager.S.IncreasePrestige();
            GameManager.S.menuWin.SetActive(false);
        }

        LevelManager.S.RoundWin();
    }

    public void btn_ReturnMainMenu()
    {
        LevelManager.S.ReturnToMainMenu();
    }
}