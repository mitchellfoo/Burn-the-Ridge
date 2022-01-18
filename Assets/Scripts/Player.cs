using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    public float MAX_OFFSET;
    public float MIN_Y;

    [Header("Game Variables")]
    public float enemyCollisionDamage = 15.0f;

    [Header("Power Up Variables")]
    public GameObject[] gunModels;
    public float healthFactor = 50.0f;
    public float regenFactor = 0.5f;
    public float speedFactor = 0.3f;

    private GameObject cannons;
    private int shootingFactor;

    [Header("Shield Variables")]
    public GameObject shield;
    public float shieldHealth = 100.0f;
    public float shieldFactor = 50.0f;
    public float shieldReloadTime = 20.0f;

    private float shieldTimer = 0.0f;
    private bool shieldUp;

    [Header("Field Variables")]
    public GameObject field;
    public string fieldKey = "f";
    public int fieldStock = 3;

    [Header("Stop Variables")]
    public GameObject stop;
    public string stopKey = "e";
    public float stopDuration = 4.0f;
    public int stopStock = 2;
    public int stopStockFactor = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Apply Upgrades
        CheckPowerups();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            Vector3 currentPosition = transform.position;

            // Directional input
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                currentPosition.x += (Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime);
                currentPosition.y += (Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);

                // check the position
                currentPosition.x = Mathf.Clamp(currentPosition.x, -MAX_OFFSET, MAX_OFFSET);
                currentPosition.y = Mathf.Clamp(currentPosition.y, MIN_Y, MAX_OFFSET * 2);

                transform.position = currentPosition;
            }

            // Check shield
            ShieldCheck();

            // Check techs
            FieldCheck();
            StopCheck();
        }
    }

    // Tech Functions
    private void FieldCheck()
    {
        if (Input.GetKeyDown(fieldKey) && fieldStock > 0)
        {
            Debug.Log("Field Used");
            SoundManager.S.MakeFieldSound();
            fieldStock--;
            GameObject currField = Instantiate(field);
            Destroy(currField, 7.0f);
        }
    }

    private void StopCheck()
    {
        if (Input.GetKeyDown(stopKey) && stopStock > 0)
        {
            Debug.Log("Stop Used");
            stopStock--;
            SoundManager.S.MakeStopSound();
            GameObject currStop = Instantiate(stop);
            Destroy(currStop, stopDuration);
            StartCoroutine(DisableEnemyAttack());
        }
    }

    private IEnumerator DisableEnemyAttack()
    {
        GameManager.S.stopEnemyAttack = true;
        Debug.Log("Attacks Disabled");
        yield return new WaitForSeconds(stopDuration);
        GameManager.S.stopEnemyAttack = false;
        Debug.Log("Attacks Enabled");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                SoundManager.S.MakePlayerDamagedSound();
                Debug.Log("Collided into Enemy!");
                StartCoroutine(GameManager.S.DamageTaken());
                GameManager.S.PlayerDamaged(enemyCollisionDamage);
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "EnemyDebris")
            {
                SoundManager.S.MakeKamiExplosionSound();
                Debug.Log("Collided into Debris!");
                StartCoroutine(GameManager.S.DamageTaken());
                GameManager.S.PlayerDamaged(collision.gameObject.GetComponent<Debris>().GetDamageValue());
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "EnemyKami")
            {
                SoundManager.S.MakeKamiExplosionSound();
                Debug.Log("Collided into Kamikaze!");
                StartCoroutine(GameManager.S.DamageTaken());
                GameManager.S.PlayerDamaged(collision.gameObject.GetComponent<Kami>().GetDamageValue());
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "EnemyLaser")
            {
                SoundManager.S.MakePlayerDamagedSound();
                StartCoroutine(GameManager.S.DamageTaken());
                GameManager.S.PlayerDamaged(collision.gameObject.GetComponent<Laser>().GetDamageValue());
                Destroy(collision.gameObject);
            }
        }
    }

    // Power Up Management
    private void CheckPowerups()
    {
        Dictionary<string, int> powerups = GameManager.S.powerups;

        foreach (string upgrade in powerups.Keys)
        {
            if ( upgrade == "Cannon")
            {
                ApplyCannon(powerups[upgrade]);
            }
            else if ( upgrade == "Spread")
            {
                ApplySpread(powerups[upgrade]);
            }
            else if (upgrade == "Damage")
            {
                ApplyDamage(powerups[upgrade]);
            }
            else if (upgrade == "Vitality")
            {
                ApplyVitality(powerups[upgrade]);
            }
            else if (upgrade == "Regen")
            {
                ApplyRegen(powerups[upgrade]);
            }
            else if (upgrade == "Shield")
            {
                ApplyShield(powerups[upgrade]);
            }
            else if (upgrade == "Speed")
            {
                ApplySpeed(powerups[upgrade]);
            }
            else if (upgrade == "Stop")
            {
                ApplyStop(powerups[upgrade]);
            }
            else if (upgrade == "Field")
            {
                ApplyField(powerups[upgrade]);
            }
        }
    }

    private void ApplyCannon(int value)
    {
        if (value >= gunModels.Length)
        {
            value = gunModels.Length - 1;
        }
        gunModels[value].SetActive(true);
        cannons = gunModels[value];
    }

    private void ApplySpread(int value)
    {
        cannons.GetComponent<Cannons>().IncreaseSpread(value*2 + 1);
    }

    private void ApplyDamage(int value)
    {
        GameManager.S.playerDamage = value + 1;
    }

    private void ApplyVitality(int value)
    {
        GameManager.S.SetMaxHealth(value * healthFactor + GameManager.S.healthStart);
    }

    private void ApplyRegen(int value)
    {
        GameManager.S.SetRegen(value * regenFactor + GameManager.S.regenStart);
    }

    private void ApplyShield(int value)
    {
        // set shield health
        shieldHealth += value * shieldFactor;

        // instantiate shield prefab
        GameObject currShield = Instantiate(shield, transform);
        currShield.GetComponent<Shield>().setShieldHealth(shieldHealth);
        shieldUp = true;
    }

    private void ShieldCheck()
    {
        if (!shieldUp)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                // instantiate shield prefab
                GameObject currShield = Instantiate(shield, transform);
                currShield.GetComponent<Shield>().setShieldHealth(shieldHealth);
                SoundManager.S.MakeShieldUpSound();

                ShieldSwitch();
            }
        }
    }

    public void ShieldSwitch()
    {
        shieldUp = !shieldUp;
        shieldTimer = shieldReloadTime;
        Debug.Log("SHIELD SWITCH");
    }

    private void ApplySpeed(int value)
    {
        speed += value * speedFactor;
    }

    private void ApplyStop(int value)
    {
        stopStock += stopStockFactor * value;
    }

    private void ApplyField (int value)
    {
        fieldStock += value;
    }
}
