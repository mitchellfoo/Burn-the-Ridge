using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S; // Singleton definition

    private AudioSource audio;

    [Header("Player Sounds")]
    public AudioSource ambientSound;

    public AudioClip trenchStartSound;

    [Header("Player Sounds")]
    public AudioClip playerFiringClip;
    public AudioClip playerExplosionClip;
    public AudioClip playerDamagedClip;
    public AudioClip shieldDamagedClip;
    public AudioClip playerShieldUpClip;
    public AudioClip playerFieldClip;
    public AudioClip playerStopClip;

    public GameObject playerShieldDownClip;
    public GameObject playerFlyingSound;

    [Header("Enemy Sounds")]
    public AudioClip enemyShootingClip;

    public GameObject enemyFlyingSound;
    public GameObject enemyExplosionPrefab;
    public GameObject kamiExplosionPrefab;


    private void Awake()
    {
        S = this; // singleton is assigned
    }

    // Start is called before the first frame update
    void Start()
    {
        // assign audio component
        audio = GetComponent<AudioSource>();
    }

    // Game Sounds

    public void MakeRoundStartSound()
    {
        audio.PlayOneShot(trenchStartSound, 0.5f);
    }

    // Enemy

    public void MakeEnemyExplosionSound()
    {
        GameObject lilBoom = Instantiate(enemyExplosionPrefab, transform);
        Destroy(lilBoom, 5.0f);
    }

    public void MakeKamiExplosionSound()
    {
        GameObject bombBoom = Instantiate(kamiExplosionPrefab, transform);
        Destroy(bombBoom, 5.0f);
    }

    public void MakeEnemyShootingSound()
    {
        audio.PlayOneShot(enemyShootingClip, 1.0f);
    }

    // Player Sounds

    public void MakePlayerDamagedSound()
    {
        audio.PlayOneShot(playerDamagedClip, 0.5f);
    }

    public void MakePlayerShootingSound()
    {
        audio.PlayOneShot(playerFiringClip, 0.3f);
    }

    public void MakeShieldDamagedSound()
    {
        audio.PlayOneShot(playerDamagedClip, 0.7f);
    }

    public void MakePlayerExplosionSound()
    {
        audio.PlayOneShot(playerExplosionClip, 1.0f);
    }

    public void MakeShieldUpSound()
    {
        audio.PlayOneShot(playerShieldUpClip, 1.5f);
    }

    public void MakeShieldDownSound()
    {
        GameObject shield = Instantiate(playerShieldDownClip, transform);
        Destroy(shield, 5.0f); ;
    }

    public void MakeFieldSound()
    {
        audio.PlayOneShot(playerFieldClip, 0.6f);
    }

    public void MakeStopSound()
    {
        audio.PlayOneShot(playerStopClip, 0.6f);
    }

    public void StopAllSounds()
    {
        // stop ambient noise
        ambientSound.Stop();

        // stop all child sounds
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }

    }
}

