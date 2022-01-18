using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherTrench : MonoBehaviour
{
    [Header("Trench Variables")]
    public GameObject defenderPrefab;
    public int defenderProbability = 20;
    public GameObject spawnerPrefab;

    [Header("Wall Variables")]
    public int wallCount = 10;
    public float wallSpacing = 5.0f;
    public GameObject[] walls;

    // Start is called before the first frame update
    void Start()
    {
        BuildInitialWalls();
        SetSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing ||
            GameManager.S.gameState == GameState.getReady ||
            GameManager.S.gameState == GameState.startMenu)
        {
            BuildNewWall();
            MoveTrench();
        }
    }
    private void SetSpawner()
    {
        Vector3 trenchEnd = transform.GetChild(transform.childCount - 1).transform.position;
        GameObject spawner = Instantiate(spawnerPrefab);
        spawner.transform.position = trenchEnd;
    }

    private void BuildInitialWalls()
    {
        for (int i = 0; i < wallCount; i++)
        {
            BuildRandWall(i);
        }
    }

    private void BuildRandWall(int dist)
    {
        // Instantiate Random Wall
        int wallIndex = Random.Range(0, walls.Length);
        GameObject wall = Instantiate(walls[wallIndex], transform);

        // Random Spawn Defender
        if (GameManager.S.gameState == GameState.playing)
        {
            AddRandDefender(wall);
        }

        // Place Wall at End
        float depth = wall.transform.GetChild(0).transform.localScale.z;
        Vector3 wallPos = wall.transform.position;
        wallPos.z += depth * dist - wallSpacing;
        wall.transform.position = wallPos;
    }

    private void AddRandDefender(GameObject wall)
    {
        int chance = Random.Range(1, 101);
        if (chance <= defenderProbability)
        {
            Instantiate(defenderPrefab, wall.transform);
        }
    }

    private void BuildNewWall()
    {
        // Check if last wall has moved far enough away
        GameObject lastWall = transform.GetChild(transform.childCount - 1).gameObject;
        float depth = lastWall.transform.GetChild(0).transform.localScale.z;
        float dist = lastWall.transform.position.z;

        if (dist < depth * (wallCount-1) - wallSpacing + 1)
        {
            BuildRandWall(wallCount);
        }
    }

    private void MoveTrench()
    {
        foreach (Transform child in transform)
        {
            // Move Child
            Vector3 newPos = child.transform.position;
            newPos.z -= GameManager.S.GetFlightSpeed();
            child.transform.position = newPos;

            // Check if child is too far back
            if (newPos.z < GameManager.S.delDist)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
