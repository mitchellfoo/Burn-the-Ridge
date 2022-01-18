using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Associated Game Objects")]
    public GameObject laserPrefab;
    
    private Transform playerTarget;
    private List<GameObject> firingPositions = new List<GameObject>();

    [Header("Game Factors")]
    public float laserDamage = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set player transform for aiming
        playerTarget = LevelManager.S.playerShip.transform;

        // Get gun pos
        SetFiringPositions();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTarget);
    } 

    private void SetFiringPositions()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Firing Position")
            {
                firingPositions.Add(child.gameObject);
            }
        }
    }

    public void FireTurrets()
    {
        if (!GameManager.S.stopEnemyAttack)
        {
            // Make Blaster Sound
            SoundManager.S.MakeEnemyShootingSound();

            // Fire Each Turret
            foreach (GameObject cannon in firingPositions)
            {
                GameObject laserObject = Instantiate(laserPrefab, (cannon.transform.position - Vector3.forward),
                                                        Quaternion.identity);
                laserObject.transform.rotation = transform.rotation;

                // Set laser damage
                float damageFactor = GameManager.S.GetDamageFactor();
                laserObject.GetComponent<Laser>().SetDamageValue(laserDamage * damageFactor);
            }
        }
    }
}
