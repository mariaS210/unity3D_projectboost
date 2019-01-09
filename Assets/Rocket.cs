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
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] ParticleSystem winEffect = null;
    [SerializeField] ParticleSystem crashEffect = null;
    [SerializeField] ParticleSystem flameEffect = null;

    enum State { Alive, Dying, Transcending};
    enum DebugCollision { On, Off};
    const string friendly = "Friendly";
    const string finish = "Finish";

    State state = State.Alive;
    DebugCollision useCollision = DebugCollision.On;
    Time time;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
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
        DebugOnInput();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        if (useCollision == DebugCollision.Off)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case friendly:
                break;
            case finish:
                Finish();
                break;
            default:
                Die();
                break;
        }
    }

    private void DebugOnInput()
    {
        if (!Debug.isDebugBuild)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Finish();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            useCollision = useCollision == DebugCollision.On ? DebugCollision.Off : DebugCollision.On;
        }
    }

    private void Finish()
    {
        state = State.Transcending;
        useCollision = DebugCollision.Off;
        //StopWithFadeOut();
        flameEffect.Stop();
        PlayWithFadeIn(nextLevel, false);
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void Die()
    {
        state = State.Transcending;
        useCollision = DebugCollision.Off;
        flameEffect.Stop();
        Invoke("LoadFirstScene", levelLoadDelay);
        //StopWithFadeOut();
        PlayWithFadeIn(crash, false);
        crashEffect.Play();
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
        int nextIndex = (currentIndex + 1) % SceneManager.sceneCountInBuildSettings;
        if (currentIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            winEffect.Play();
        }
        SceneManager.LoadScene(nextIndex);
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
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            RotateNoPhysics(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateNoPhysics(-rotationThisFrame);
        }
    }

    private void RotateNoPhysics(float rotationThisFrame)
    {
        // take manual control of rotation
        rigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        // resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

    private void SoundsOnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayWithFadeIn(mainEngine);   
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopWithFadeOut();
            flameEffect.Stop();
        }
    }

    private void PlayWithFadeIn(AudioClip sound, bool loop=true)
    {
        //if (!rocketSound.isPlaying)
        //{
            //rocketSound.FadeOut(1f);
            rocketSound.clip = sound;
            rocketSound.loop = loop;
            rocketSound.volume = 1f;
            //rocketSound.FadeIn(2f);
            rocketSound.Play();
        //}
    }

    private void StopWithFadeOut()
    {
        //if (rocketSound.isPlaying)
        //{
            rocketSound.FadeOut(0.1f);
            //rocketSound.Stop();
            rocketSound.loop = false;
        //}
    }
}
