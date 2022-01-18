using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kami : MonoBehaviour
{
    [Header("Movement Variables")]
    public float flySpeed = .7f;

    [Header("Game Variables")]
    public int health = 1;
    public int scoreValue = 500;
    public float damageValue = 20.0f;

    private Transform playerTarget;

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = LevelManager.S.playerShip.transform;
        GameManager.S.AddToMaxScore(scoreValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            // Move
            transform.LookAt(playerTarget);
            transform.position += transform.forward * flySpeed;
        }

        // Delete in Case Misses?
        if (transform.position.z <= GameManager.S.delDist)
        {
            Destroy(this.gameObject);
        }
    }

    public float GetDamageValue()
    {
        return damageValue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerLaser")
        {
            Debug.Log("Waver Hit");
            health -= GameManager.S.playerDamage;
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                // explode the object
                //Explode();

                SoundManager.S.MakeKamiExplosionSound();

                GameManager.S.AddToScore(scoreValue);

                // destroy self
                Destroy(this.gameObject);
            }
        }
    }
}
