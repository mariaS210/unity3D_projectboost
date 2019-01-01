using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource rocketSound;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = 1;
        rocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementOnInput();
        SoundsOnInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        const string friendly = "Friendly";
        switch (collision.gameObject.tag)
        {
            case friendly:
                print("OK");
                break;
            default:
                print("Dead");
                break;
        }
    }

    private void MovementOnInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        }

        // take manual control of rotation
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating right");
            transform.Rotate(-Vector3.forward * rotationThisFrame);
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
