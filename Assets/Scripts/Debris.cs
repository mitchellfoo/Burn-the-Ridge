using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [Header("Game Variables")]
    public int health = 10;
    public int scoreValue = 100;
    public float damageValue = 15.0f;
    public int torqueRange = 60;

    private Vector3 torque;

    void Start()
    {
        torque.x = Random.Range(-torqueRange, torqueRange);
        torque.y = Random.Range(-torqueRange, torqueRange);
        torque.z = Random.Range(-torqueRange, torqueRange);
        GetComponent<ConstantForce>().torque = torque;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            // Move
            Vector3 newPos = transform.position;
            newPos.z -= GameManager.S.GetFlightSpeed();
            transform.position = newPos;
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
            // destroy the bullet
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
