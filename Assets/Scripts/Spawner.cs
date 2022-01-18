using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Associated Game Objects")]
    public GameObject[] enemyPrefabs;

    [Header("Spawning Behavior")]
    public float spawnInterval = 10.0f;

    private List<int[]> levels = new List<int[]>();
    private int[] waves;
    private int currRound;
    private List<GameObject> spawns = new List<GameObject>();
    private int spawnCount = 0;

    private float spawnTimer = 0.0f;
    private List<int[]>[] spawnDifficulties = new List<int[]>[3]; // Set to 3 total difficulties
    private List<int[]> spawnsEasy = new List<int[]>();
    private List<int[]> spawnsMedium = new List<int[]>();
    private List<int[]> spawnsHard = new List<int[]>();

    // Start is called before the first frame update
    void Start()
    {
        currRound = LevelManager.S.level;
        SetSpawners();
        SetWaves();
        SetSpawnPatterns();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            UpdateSpawnTimer();
        }
    }

    private void SetSpawners()
    {
        foreach (Transform spawn in transform)
        {
            spawns.Add(spawn.gameObject);
        }
    }

    private void SetWaves()
    {
        // Design waves per levels
        /*
         * 0 is Easy
         * 1 is Medium
         * 2 is Hard
         */

        /*
        levels.Add(new int[] { 0, 0, 0, 1, 0, 1, 0, 1, 2, 1, 2 });
        levels.Add(new int[] { 0, 1, 1, 2, 0, 1, 2, 0, 1, 2, 2 });
        levels.Add(new int[] { 1, 1, 2, 2, 2, 1, 0, 2, 2, 1, 2 });
        */

        // For Testing
        levels.Add(new int[] { 0 });
        levels.Add(new int[] { 1 });
        levels.Add(new int[] { 2 });
        //*/

        waves = levels[currRound];
    }

    private void SetSpawnPatterns()
    {
        // Design pool of wave combos per difficulty
        
        /*
         * 0 is null spawn
         * 1 is kami spawn
         * 2 is dart spawn
         * 3 is debris spawn
         * 4 is horizontal waver spawn
         * 5 is vertical waver spawn
         * 6 is right waver spawn
         * 7 is left  waver spawn
         */

        // Easy
        spawnsEasy.Add(new int[]{ 2, 0, 2,
                                  0, 0, 0,
                                  2, 0, 2 });
        spawnsEasy.Add(new int[]{ 4, 0, 0,
                                  0, 0, 0,
                                  0, 0, 4 });
        spawnsEasy.Add(new int[]{ 6, 0, 0,
                                  0, 0, 0,
                                  0, 0, 0 });
        spawnsEasy.Add(new int[]{ 6, 0, 0,
                                  0, 0, 0,
                                  0, 0, 0 });
        spawnsEasy.Add(new int[]{ 0, 2, 0,
                                  0, 3, 0,
                                  0, 0, 0 });

        // Medium
        spawnsMedium.Add(new int[]{ 5, 0, 0,
                                    0, 2, 0,
                                    0, 0, 5 });
        spawnsMedium.Add(new int[]{ 0, 0, 0,
                                    0, 1, 0,
                                    0, 0, 0 });
        spawnsMedium.Add(new int[]{ 2, 0, 7,
                                    0, 0, 0,
                                    0, 0, 2 });
        spawnsMedium.Add(new int[]{ 0, 3, 0,
                                    0, 4, 0,
                                    0, 3, 0 });

        // Hard
        spawnsHard.Add(new int[]{ 4, 0, 0,
                                  2, 1, 2,
                                  0, 0, 4 });
        spawnsHard.Add(new int[]{ 2, 2, 2,
                                  2, 2, 2,
                                  2, 2, 2 });
        spawnsHard.Add(new int[]{ 6, 2, 0,
                                  2, 7, 2,
                                  0, 2, 0 });
        spawnsHard.Add(new int[]{ 3, 1, 3,
                                  0, 3, 0,
                                  3, 1, 3 });


        // Add to spawn array
        spawnDifficulties[0] = spawnsEasy;
        spawnDifficulties[1] = spawnsMedium;
        spawnDifficulties[2] = spawnsHard;
    }

    private void UpdateSpawnTimer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            // Update timer
            spawnTimer -= spawnInterval;

            // Check current spawn
            if (spawnCount == waves.Length)
            {
                StartCoroutine(GameManager.S.LevelComplete());
            }
            else
            {
                // Spawn a selected wave
                SpawnWave();
                spawnCount++;
            }
        }
    }

    private int[] SelectWave()
    {
        int currDifficulty = waves[spawnCount];
        int choice = Random.Range(0, spawnDifficulties[currDifficulty].Count);
        Debug.Log(currDifficulty + ":" + choice);
        return spawnDifficulties[currDifficulty][choice];
    }

    private void SpawnWave()
    {
        int[] wave = SelectWave();

        for (int i = 0; i < wave.Length; i++)
        {
            int enemy = wave[i];
            if (enemy != 0)
            {
                if (enemy == 1 || enemy == 2 || enemy == 3)
                {
                    Instantiate(enemyPrefabs[enemy], spawns[i].transform);
                }
                else if (enemy >= 4)
                {
                    GameObject waver = Instantiate(enemyPrefabs[4], spawns[i].transform);
                    SetWaverPattern(waver, enemy);
                }
            }
        }
    }

    private void SetWaverPattern(GameObject waver, int type)
    {
        Waver.MovePattern pattern = Waver.MovePattern.horizontal;

        if (type == 5)
        {
            pattern = Waver.MovePattern.vertical;
        }
        else if (type == 6)
        {
            pattern = Waver.MovePattern.diagonalLeft;
        }
        else if (type == 7)
        {
            pattern = Waver.MovePattern.diagonalRight;
        }

        waver.GetComponent<Waver>().SetPattern(pattern);
    }
}
