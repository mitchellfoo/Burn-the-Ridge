using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyLaser")
        {
            Destroy(collision.gameObject);
            Debug.Log("Shield at Work!");
        }
    }
}
