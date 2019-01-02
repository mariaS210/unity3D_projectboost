using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    AudioSource rocketSound;
    // will be set in editor
    [SerializeField] AudioClip mainEngine = null;
    [SerializeField] AudioClip nextLevel = null;
    [SerializeField] AudioClip crash = null;

    [SerializeField] ParticleSystem winEffect = null;
    [SerializeField] ParticleSystem crashEffect = null;
    [SerializeField] ParticleSystem flameEffect = null;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
    int finalScene = 1;

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
        if (state != State.Alive)
        {
            return;
        }
        MovementOnInput();
        SoundsOnInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        const string friendly = "Friendly";
        const string finish = "Finish";
        switch (collision.gameObject.tag)
        {
            case friendly:
                print("OK");
                break;
            case finish:
                Finish();
                break;
            default:
                Die();
                break;
        }
    }

    private void Finish()
    {
        state = State.Transcending;
        rocketSound.PlayOneShot(nextLevel);
        rocketSound.loop = false;
        Invoke("LoadNextScene", 1f);
    }

    private void Die()
    {
        print("Dead");
        state = State.Transcending;
        Invoke("LoadFirstScene", 1f);
        rocketSound.Stop();
        rocketSound.PlayOneShot(crash);
        crashEffect.Play();
        rocketSound.loop = false;
    }

    private void LoadFirstScene()
    {
        state = State.Dying;
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        int currentIndex = activeScene != null ? activeScene.buildIndex : 0;
        int nextIndex = currentIndex + 1;
        if (nextIndex > finalScene)
        {
            print("Win!!!");
            winEffect.Play();
        }
        else
        {
            SceneManager.LoadScene(nextIndex);
        }
        state = State.Alive;
    }

    private void MovementOnInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            flameEffect.Play();
        }

        // take manual control of rotation
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        // resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

    private void SoundsOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.PlayOneShot(mainEngine);
            rocketSound.loop = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
            flameEffect.Stop();
        }
    }
}
