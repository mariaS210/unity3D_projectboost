using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource rocketSound;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
         ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            // print("Thrusting");
            rigidBody.AddRelativeForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            transform.Rotate(Vector3.forward * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating right");
            transform.Rotate(-Vector3.forward * Time.deltaTime);
        }
    }
}
