using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { startMenu, pauseMenu, upgradeMenu, getReady, playing, roundWin, gameOver, gameWin }

public class GameManager : MonoBehaviour
{
    // Singleton Definition
    public static GameManager S;

    [Header("Game State")]
    public GameState gameState;
    public float readyStateDuration = 2.0f;
    public bool stopEnemyAttack = false;

    private int currLevelScene = 0;

    [Header("UI Variables")]
    public TextMeshProUGUI messageOverlay;
    public TextMeshProUGUI scoreText1;
    public TextMeshProUGUI healthText1;
    public TextMeshProUGUI scoreText2;
    public TextMeshProUGUI healthText2;
    public string pauseKey = "p";

    public GameObject menuReturn;
    public GameObject menuPause;
    public GameObject menuWin;

    [Header("Flight Variables")]
    public float delDist = -25.0f;
    public float flySpeed = 0.5f;

    private float speedFactor = 1.0f;

    [Header("Enemy Variables")]
    private float damageFactor = 1.0f;

    [Header("Player Variables")]
    public GameObject damageTaken;
    public float healthStart = 200.0f;
    public float regenStart = 1.0f;
    public float regenTimer = 1.0f;
    public int playerDamage = 1;

    private float maxHealth;
    private float health;
    private float healthRegen;
    private int gameScore = 0;
    private int roundScore = 0;
    private int maxRoundScore = 0;
    private int currPrestige = 0;

    // Powerup Dictionary
    public Dictionary<string, int> powerups = new Dictionary<string, int>()
    {
        { "Cannon", 4 },
        { "Spread", 4 },
        { "Damage", 0 },
        { "Vitality", 0 },
        { "Regen", 0 },
        { "Shield", 0 },
        { "Speed", 0 },
        { "Stop", 0 },
        { "Field", 0 }
    };

    public string[] gunsUpgrades = { "Cannon", "Spread", "Damage" };
    public string[] shipUpgrades = { "Vitality", "Regen", "Shield" };
    public string[] techUpgrades = { "Speed", "Stop", "Field" };

    private void Awake()
    {
        //Singleton def
        if (GameManager.S)
        {
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        StartNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.playing)
        {
            CheckPauseGame();
            HealthRegen();
        }
    }

    // Game State Functions
    private void StartNewGame()
    {
        // state
        gameState = GameState.startMenu;
        Time.timeScale = 1;

        // reset lives
        health = healthStart;
        maxHealth = healthStart;
        healthRegen = regenStart;

        // reset score
        gameScore = 0;
        roundScore = 0;
        currPrestige = 0;

        // text
        scoreText2.text = "" + roundScore;
        healthText2.text = "" + health;
    }

    public void ResetRound()
    {
        // Menus
        menuWin.SetActive(false);
        damageTaken.SetActive(false);

        // State checks
        if (!LevelManager.S.titleScene && !LevelManager.S.powerupScene)
        {
            scoreText1.enabled = true;
            healthText1.enabled = true;
            scoreText2.enabled = true;
            healthText2.enabled = true;
        }
        else
        {
            scoreText1.enabled = false;
            healthText1.enabled = false;
            scoreText2.enabled = false;
            healthText2.enabled = false;
        }

        // start the get ready coroutine
        StartCoroutine(GetReadyState());
    }
    

    public IEnumerator GetReadyState()
    {
        // Set State
        if (LevelManager.S.titleScene)
        {
            gameState = GameState.startMenu;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (LevelManager.S.powerupScene)
        {
            gameState = GameState.upgradeMenu;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            gameState = GameState.getReady;
        }
        Debug.Log(gameState);

        // Reset round score
        roundScore = 0;
        maxRoundScore = 0;

        // pause for 2 seconds
        yield return new WaitForSeconds(readyStateDuration);

        // turn of message
        //messageOverlay.enabled = false;
        gameState = GameState.playing;
        SoundManager.S.MakeRoundStartSound();
    }

    private void CheckPauseGame()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            gameState = GameState.pauseMenu;
            menuPause.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        gameState = GameState.playing;
        menuPause.SetActive(false);
    }

    public void PlayerDamaged(float damage)
    {
        // go boom
        SoundManager.S.MakePlayerDamagedSound();

        // remove health
        health -= damage;
        healthText2.text = "" + health;

        // check alive
        if (health <= 0)
        {
            StartCoroutine(LoseState());
        }
    }

    private IEnumerator LoseState()
    {
        // go boom
        SoundManager.S.StopAllSounds();
        SoundManager.S.MakePlayerExplosionSound();

        // UI reaction
        yield return new WaitForSeconds(0.1f);
        gameScore += roundScore;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        gameState = GameState.gameOver;
        menuReturn.SetActive(true);
    }

    public IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(15.0f);
        Cursor.lockState = CursorLockMode.None;
        gameScore += roundScore;
        if (LevelManager.S.finalScene)
        {
            currLevelScene = 0;
            gameState = GameState.gameWin;
            menuWin.SetActive(true);
        }
        else
        {
            gameState = GameState.roundWin;
            LevelManager.S.RoundWin();
        }
    }

    public void GameComplete()
    {
        // Function called when current wave is completed and that was the last level
        Cursor.lockState = CursorLockMode.None;
        gameState = GameState.gameWin;
        menuWin.SetActive(true);
    }

    public IEnumerator DamageTaken()
    {
        damageTaken.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        damageTaken.SetActive(false);
    }

    // Getter Setter Functions
    public int GetGameScore()
    {
        return gameScore;
    }

    public int GetRoundScore()
    {
        return roundScore;
    }

    public int GetMaxRoundScore()
    {
        return maxRoundScore;
    }

    public void AddToScore(int enemyScore)
    {
        roundScore += enemyScore;
        scoreText2.text = "" + roundScore;
    }

    public void AddToMaxScore(int enemyScore)
    {
        maxRoundScore += enemyScore;
    }

    public void AddHealth(float healthAdd)
    {
        health += healthAdd;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthText2.text = "" + health;
    }

    public void SetMaxHealth(float healthAdd)
    {
        maxHealth = healthAdd;
        AddHealth(healthAdd);
    }

    public void SetRegen(float regenAdd)
    {
        healthRegen = regenAdd;
    }

    public float GetFlightSpeed()
    {
        return flySpeed * speedFactor;
    }

    public void IncreaseFlightSpeed(int add = 1)
    {
        speedFactor += 0.1f * add;
    }

    public float GetDamageFactor()
    {
        return damageFactor;
    }

    public void IncreaseDamageFactor(int add = 1)
    {
        damageFactor += 0.2f * add;
    }

    public int GetPrestige()
    {
        return currPrestige;
    }

    public void IncreasePrestige()
    {
        currPrestige++;
    }

    public int GetScene()
    {
        return currLevelScene;
    }

    public void IncreaseCurrLevel()
    {
        currLevelScene++;
    }

    // Power Ups
    private void HealthRegen()
    {
        if (regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
        }
        else
        {
            AddHealth(healthRegen);
            regenTimer = 1.0f;
        }
    }

}
