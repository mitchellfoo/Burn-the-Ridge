using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [Header("Attack Variables")]
    public float attackInterval = 0.8f;
    public float tooClose = 30.0f;
    public float tooFar = 500.0f;

    private List<GameObject> guns = new List<GameObject>();
    private float attackTimer = 0.0f;

    [Header("Game Variables")]
    public int health = 3;
    public int scoreValue = 200;

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

                SoundManager.S.MakeEnemyExplosionSound();

                GameManager.S.AddToScore(scoreValue);

                // destroy self
                Destroy(this.gameObject);
            }
        }
    }
}
