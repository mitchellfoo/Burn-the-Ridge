using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;

    [Header("Level Info")]
    public string levelName; // string to display at level start
    public int level = 0;

    private string[] sceneNames = {"Trench1", "Trench2", "Trench3"};

    [Header("Game Objects")]
    public GameObject playerShip;

    [Header("Scene Info")]
    public string nextScene; // string of level name
    public bool titleScene;
    public bool powerupScene;
    public bool finalScene; // end of game


    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        if (GameManager.S)
        {
            if (powerupScene)
            {

            }
            else
            {
                GameManager.S.ResetRound();
            }
        }
    }

    public void RoundWin()
    {
        if (powerupScene)
        {
            GameManager.S.IncreaseCurrLevel();
            Debug.Log(sceneNames[GameManager.S.GetScene()]);
            SceneManager.LoadScene(sceneNames[GameManager.S.GetScene()]);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void GameWin()
    {
        SceneManager.LoadScene("WinMenu");
    }

    public void RestartLevel()
    {
        // reload this scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("TitleScene");
    }

}
