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
        MovementOnInput();
        SoundsOnInput();
    }

    private void MovementOnInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }
        // take manual control of rotation
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating right");
            transform.Rotate(-Vector3.forward);
        }
        // resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

    private void SoundsOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
        }
    }
}
