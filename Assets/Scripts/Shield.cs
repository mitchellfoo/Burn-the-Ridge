using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Shield Variables")]
    public GameObject panel;

    private float shieldHealth = 100.0f;

    // Update is called once per frame
    void Update()
    {
        if (shieldHealth <= 0)
        {
            SoundManager.S.MakeShieldDownSound();
            transform.parent.GetComponent<Player>().ShieldSwitch();
            Destroy(this.gameObject);
        }
    }

    public void setShieldHealth(float health)
    {
        shieldHealth = health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SoundManager.S.MakeShieldDamagedSound();
            shieldHealth -= transform.parent.GetComponent<Player>().enemyCollisionDamage;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "EnemyDebris")
        {
            SoundManager.S.MakeShieldDamagedSound();
            shieldHealth -= collision.gameObject.GetComponent<Debris>().GetDamageValue();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "EnemyKami")
        {
            SoundManager.S.MakeShieldDamagedSound();
            shieldHealth -= collision.gameObject.GetComponent<Kami>().GetDamageValue();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "EnemyLaser")
        {
            SoundManager.S.MakeShieldDamagedSound();
            shieldHealth -= collision.gameObject.GetComponent<Laser>().GetDamageValue();
            Destroy(collision.gameObject);
        }
    }
}
