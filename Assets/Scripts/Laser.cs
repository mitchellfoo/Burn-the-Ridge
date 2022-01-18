using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;

    private float damageValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
        {
            transform.position = transform.position + transform.rotation * Vector3.forward * speed;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Z Pos check
        if (transform.position.z < GameManager.S.delDist)
        {
            Destroy(gameObject);
        }
    }

    public float GetDamageValue()
    {
        return damageValue;
    }

    public void SetDamageValue(float damVal)
    {
        damageValue = damVal;
    }
}
