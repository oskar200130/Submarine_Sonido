/// <summary>
/// Sub controller script
/// /2017 Dylan Papp dylanpapp.daportfolio.com
/// </summary>


using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subController : MonoBehaviour
{

    private Transform selfTransform;
    private Rigidbody selfRigidbody;

    float throttle;
    float yaw;
    float pitch;
    float roll;
    public float throttleMax = 5f;
    public float yawMax = 1f;
    public float pitchMax = 1f;
    public float rollMax = 1f;
    public float throttleRate = 1f;
    public float turnRate = 1f;
    public float decreaseRate = 0.1f;
    public GameObject spot;
    public GameObject surface;
    public ParticleSystem bubbleFX;

    public Transform propellerTransform;
    private SubmarineDamageManager subDamMng;

    private bool input = true;
    public float timeStuned = 5.0f;
    private float timer = 0;
    private FMOD.Studio.EventInstance engineSound;
    public FMODUnity.EventReference engineSoundRef;


    void Start()
    {
        selfTransform = transform;
        selfRigidbody = transform.GetComponent<Rigidbody>();
        subDamMng = transform.GetComponent<SubmarineDamageManager>();
        engineSound = RuntimeManager.CreateInstance(engineSoundRef);
        RuntimeManager.AttachInstanceToGameObject(engineSound, propellerTransform);
        engineSound.start();
    }

    private void OnEnable()
    {
        engineSound.start();
    }

    private void OnDisable()
    {
        engineSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Update()
    {
        if (input)
        {
            //Light
            if (Input.GetKeyDown("l"))
                spot.SetActive(!spot.activeSelf);

            //Throttle------------------------------------------------
            if (Input.GetAxis("Throttle") >= 0.01)
            {
                throttle += Input.GetAxis("Throttle") * throttleRate * Time.deltaTime;
                throttle = Mathf.Clamp(throttle, 0, throttleMax);
            }
            else if (Input.GetAxis("Throttle") <= -0.01)
            {
                throttle += Input.GetAxis("Throttle") * throttleRate * Time.deltaTime;
                throttle = Mathf.Clamp(throttle, -throttleMax, 0);
            }
            else
            {
                if (throttle < 0)
                {
                    throttle += decreaseRate * Time.deltaTime;
                    throttle = Mathf.Clamp(throttle, -throttleMax, 0);
                }
                else if (throttle > 0)
                {
                    throttle -= decreaseRate * Time.deltaTime;
                    throttle = Mathf.Clamp(throttle, 0, throttleMax);
                }

            }

            //Yaw---------------------------------------
            if (Input.GetAxis("Yaw") >= 0.01)
            {
                yaw += Input.GetAxis("Yaw") * turnRate * Time.deltaTime;
                yaw = Mathf.Clamp(yaw, 0, yawMax);
            }
            else if (Input.GetAxis("Yaw") <= -0.01)
            {
                yaw += Input.GetAxis("Yaw") * turnRate * Time.deltaTime;
                yaw = Mathf.Clamp(yaw, -yawMax, 0);
            }
            else
            {
                if (yaw < 0)
                {
                    yaw += decreaseRate * Time.deltaTime;
                    yaw = Mathf.Clamp(yaw, -yawMax, 0);
                }
                else if (yaw > 0)
                {
                    yaw -= decreaseRate * Time.deltaTime;
                    yaw = Mathf.Clamp(yaw, 0, yawMax);
                }

            }

            //Pitch-----------------------------------------
            if (Input.GetAxis("Pitch") >= 0.01)
            {
                pitch += Input.GetAxis("Pitch") * turnRate * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, 0, pitchMax);
            }
            else if (Input.GetAxis("Pitch") <= -0.01)
            {
                pitch += Input.GetAxis("Pitch") * turnRate * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -pitchMax, 0);
            }
            else
            {
                if (pitch < 0)
                {
                    pitch += decreaseRate * Time.deltaTime;
                    pitch = Mathf.Clamp(pitch, -pitchMax, 0);
                }
                else if (pitch > 0)
                {
                    pitch -= decreaseRate * Time.deltaTime;
                    pitch = Mathf.Clamp(pitch, 0, pitchMax);
                }
            }

            //Roll-----------------------------------------
            if (Input.GetAxis("Roll") >= 0.01)
            {
                roll += Input.GetAxis("Roll") * turnRate * Time.deltaTime;
                roll = Mathf.Clamp(roll, 0, rollMax);
            }
            else if (Input.GetAxis("Roll") <= -0.01)
            {
                roll += Input.GetAxis("Roll") * turnRate * Time.deltaTime;
                roll = Mathf.Clamp(roll, -rollMax, 0);
            }
            else
            {
                if (roll < 0)
                {
                    roll += decreaseRate * Time.deltaTime;
                    roll = Mathf.Clamp(roll, -rollMax, 0);
                }
                else if (roll > 0)
                {
                    roll -= decreaseRate * Time.deltaTime;
                    roll = Mathf.Clamp(roll, 0, rollMax);
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && subDamMng.getTotalDamg() < 80)
            {
                input = true;
                timer = 0;
                selfRigidbody.drag = 0.01f;
                selfRigidbody.angularDrag = 0.01f;
                selfRigidbody.velocity = Vector3.zero;
                transform.GetComponent<SubmarineDamageManager>().TurnOffAlarm();
            }
        }

        engineSound.setParameterByName("SubVelocity", Math.Abs(throttle) * 10 / throttleMax);
    }

    void FixedUpdate()
    {
        var bblFX = bubbleFX.main;

        propellerTransform.rotation *= Quaternion.AngleAxis((throttle * 320.0f) * Time.deltaTime, Vector3.forward);

        bblFX.startSize = 0.05f * throttle;
        bblFX.startSpeed = 2f * throttle;

        selfTransform.Translate(throttle * Vector3.forward);
        selfTransform.Rotate(yaw * Vector3.up);
        if (throttle == 0)
            selfTransform.Translate(pitch * Vector3.down / 4.0f);
        else
            selfTransform.Rotate(pitch * Vector3.right);
        selfTransform.Rotate(roll * Vector3.forward);

        if (selfTransform.position.y >= surface.transform.position.y)
        {
            selfTransform.position = new Vector3(selfTransform.position.x, surface.transform.position.y, selfTransform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        input = false;
        selfRigidbody.drag = 1.5f;
        selfRigidbody.angularDrag = 1.5f;
        timer = Math.Abs(throttle) / throttleMax * timeStuned;
        selfRigidbody.velocity = -Math.Abs(throttle) * selfTransform.forward * 60;
        subDamMng.recieveDamage(Math.Abs(throttle) / throttleMax * 20);
        throttle = pitch = roll = yaw = 0.0f;
    }
}
