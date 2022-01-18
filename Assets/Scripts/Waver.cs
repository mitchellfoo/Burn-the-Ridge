using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waver : MonoBehaviour
{
    [Header("Movement Variables")]
    public float flySpeed = 0.5f;
    public float patternSpeed = 0.5f;
    public float TRENCH_PADDING = 10.0f;
    public float TRENCH_HEIGHT = 75.0f;
    public float TRENCH_WIDTH = 100.0f;
    public enum MovePattern { horizontal, vertical, diagonalLeft, diagonalRight }
    public MovePattern pattern;

    private Vector3 moveVector1;
    private Vector3 moveVector2;
    private Vector3 currDirection;

    [Header("Attack Variables")]
    public float attackInterval = 0.8f;
    public float tooClose = 80.0f;
    public float tooFar = 800.0f;

    private List<GameObject> guns = new List<GameObject>();
    private float attackTimer = 0.0f;

    [Header("Game Variables")]
    public int health = 7;
    public int scoreValue = 800;

    // Start is called before the first frame update
    void Start()
    {
        GetGuns();
        GameManager.S.AddToMaxScore(scoreValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            UpdateFireTimer();
            MoveWaver();
            CheckDirectionSwitch();
        }
    }

    public void SetPattern(MovePattern newPattern)
    {
        pattern = newPattern;
        SetDirection();
    }

    private void SwitchDirection()
    {
        if (currDirection == moveVector1)
        {
            currDirection = moveVector2;
        }
        else
        {
            currDirection = moveVector1;
        }
    }

    private void SetDirection()
    {
        if (pattern == MovePattern.horizontal)
        {
            moveVector1 = Vector3.right;
            moveVector2 = Vector3.left;
        }
        else if (pattern == MovePattern.vertical)
        {
            moveVector1 = Vector3.up;
            moveVector2 = Vector3.down;
        }
        else if (pattern == MovePattern.diagonalLeft)
        {
            moveVector1 = Vector3.up + Vector3.left;
            moveVector2 = Vector3.down + Vector3.right;
        }
        else if (pattern == MovePattern.diagonalRight)
        {
            moveVector1 = Vector3.up + Vector3.right;
            moveVector2 = Vector3.down + Vector3.left;
        }

        if (GreaterThanCenter())
        {
            currDirection = moveVector2;
        }
        else
        {
            currDirection = moveVector1;
        }
    }

    private bool GreaterThanCenter()
    {
        if (pattern == MovePattern.vertical)
        {
            return transform.position.y > TRENCH_WIDTH / 2;
        }
        else
        {
            return transform.position.x > TRENCH_HEIGHT / 2;
        }
    }

    private void GetGuns()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Cannon")
            {
                guns.Add(child.gameObject);
            }
        }
    }

    private Guns GetGunTurrets(GameObject gun)
    {
        return gun.GetComponent<Guns>();
    }

    private void FireTurrets()
    {
        foreach (GameObject gun in guns)
        {
            GetGunTurrets(gun).FireTurrets();
        }
    }

    private void UpdateFireTimer()
    {
        attackTimer += Time.deltaTime;
        bool canAttack = transform.position.z > tooClose && transform.position.z < tooFar;
        if (attackTimer >= attackInterval)
        {
            attackTimer -= attackInterval;
            if (GameManager.S.gameState == GameState.playing && canAttack)
            {
                FireTurrets();
            }
        }
    }

    private void CheckDirectionSwitch()
    {
        if (pattern == MovePattern.horizontal)
        {
            if (transform.position.x > TRENCH_WIDTH / 2 - TRENCH_PADDING ||
                transform.position.x < -TRENCH_WIDTH / 2 + TRENCH_PADDING)
            {
                SwitchDirection();
            }
        }
        else if (pattern == MovePattern.vertical)
        {
            if (transform.position.y > TRENCH_HEIGHT - TRENCH_PADDING ||
                transform.position.y < 0 + TRENCH_PADDING)
            {
                SwitchDirection();
            }
        }
        else
        {
            if (transform.position.y > TRENCH_HEIGHT - TRENCH_PADDING ||
                transform.position.y < 0 + TRENCH_PADDING)
            {
                SwitchDirection();
            }
            else if (transform.position.x > TRENCH_WIDTH / 2 - TRENCH_PADDING ||
                transform.position.x < -TRENCH_WIDTH / 2 + TRENCH_PADDING)
            {
                SwitchDirection();
            }
        }
    }

    private void MoveWaver()
    {
        // Move by direction
        Vector3 newPos = transform.position;
        newPos.z -= flySpeed;
        newPos += currDirection * patternSpeed;
        transform.position = newPos;

        // Check if too far back
        if (newPos.z < GameManager.S.delDist)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerLaser")
        {
            Debug.Log("Dart Hit");
            health -= GameManager.S.playerDamage;
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                // explode the object
                //Explode();

                SoundManager.S.MakeEnemyExplosionSound();

                GameManager.S.AddToScore(scoreValue);

                // destroy self
                Destroy(this.gameObject);
            }
        }
    }
}
