using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannons : MonoBehaviour
{
    [Header("Associated Game Objects")]
    public GameObject laserPrefab;

    [Header("Aiming Variables")]
    public float mouseSpeed = 100.0f;
    public float maxTilt = 10.0f;
    public float maxRotate = 5.0f;

    [Header("Firing Variables")]
    public float laserLifetime = 5.0f;
    public float spreadAngle = 3.0f;

    private int spreadFire = 1;
    private float tiltRotation = 0.0f;
    private float cannonRotation = 0.0f;
    private List<GameObject> firingPositions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.S.gameState == GameState.playing || GameManager.S.gameState == GameState.getReady)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        SetFiringPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing || GameManager.S.gameState == GameState.getReady)
        {
            // Cannon Aiming
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            /// x rotation
            cannonRotation += mouseX * mouseSpeed * Time.deltaTime;
            cannonRotation = Mathf.Clamp(cannonRotation, -maxRotate, maxRotate);

            /// y tilt rotation
            tiltRotation -= mouseY * mouseSpeed * Time.deltaTime;
            tiltRotation = Mathf.Clamp(tiltRotation, -maxTilt, maxTilt);
            transform.localRotation = Quaternion.Euler(tiltRotation, cannonRotation, 0);

            // Cannon Firing
            if (Input.GetMouseButtonDown(0))
            {
                FireCannons();
            }
        }
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

    public void IncreaseSpread(int increase)
    {
        spreadFire = increase;
    }

    private void FireCannons()
    {
        foreach (GameObject cannon in firingPositions)
        {
            if (GameManager.S.gameState == GameState.playing)
            {
                SoundManager.S.MakePlayerShootingSound();
            }
            for (int i = 0; i < spreadFire; i ++)
            {
                Quaternion fireDirection = Quaternion.identity;

                if (i == 1)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(0f, spreadAngle, 0f);
                }
                else if (i == 2)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(0f, -spreadAngle, 0f);
                }
                else if (i == 3)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(spreadAngle, 0f, 0f);
                }
                else if (i == 4)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(-spreadAngle, 0f, 0f);
                }
                else if (i == 5)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(spreadAngle, spreadAngle, 0f);
                }
                else if (i == 6)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(-spreadAngle, spreadAngle, 0f);
                }
                else if (i == 7)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(spreadAngle, -spreadAngle, 0f);

                }
                else if (i >= 8)
                {
                    fireDirection = transform.rotation * Quaternion.Euler(-spreadAngle, -spreadAngle, 0f);
                }

                GameObject laserObject = Instantiate(laserPrefab, (cannon.transform.position + Vector3.forward),
                                                         Quaternion.identity);
                laserObject.transform.rotation = transform.rotation * fireDirection;
                Destroy(laserObject, laserLifetime);
            }
        }
    }
}
