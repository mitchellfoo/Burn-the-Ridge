using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPS : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;

    private void Start()
    {
        //controller = gameObject.AddComponent<CharacterController>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Spacebar control
        if (Input.GetButtonDown("Jump"))
        {

        }

        controller.Move(playerVelocity * Time.deltaTime);
    }
}
