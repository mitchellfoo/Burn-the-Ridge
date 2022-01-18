using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [Header("Movement Variables")]
    public float flySpeed = 1.0f;

    [Header("Attack Variables")]
    public float attackInterval = 3.0f;
    public float tooClose = 50.0f;
    public float tooFar = 1000.0f;
    public int attackBurst = 3;
    public float burstInterval = 0.1f;

    private List<GameObject> guns = new List<GameObject>();
    private float attackTimer = -1.0f;

    [Header("Game Variables")]
    public int health = 5;
    public int scoreValue = 500;

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
            MoveDart();
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

    private Turret GetGunTurrets(GameObject gun)
    {
        return gun.GetComponent<Turret>();
    }

    private IEnumerator FireTurrets()
    {
        WaitForSeconds delay = new WaitForSeconds(burstInterval);
        for (int i = 0; i < attackBurst; i++) 
        {
            foreach (GameObject gun in guns)
            {
                GetGunTurrets(gun).FireTurrets();
            }
            yield return delay;
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
                StartCoroutine(FireTurrets());
            }
        }
    }

    private void MoveDart()
    {
        Vector3 newPos = transform.position;
        newPos.z -= flySpeed;
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
            // destroy the bullet
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                // explode the object
                //Explode();

                //SoundManager.S.MakeEnemyExplosionSound();

                GameManager.S.AddToScore(scoreValue);

                // destroy self
                Destroy(this.gameObject);
            }
        }
    }
}
