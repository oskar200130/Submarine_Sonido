using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController cc;
    public float Velocidad = 12;

    public float Gravedad = -9.81f;
    private Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask floorMask;
    private bool isGrounded;
    public bool inSubmarine;
    public LightingManager lM;

    public Transform waterSurface;

    private FMOD.Studio.EventInstance swimInstance;
    public FMODUnity.EventReference swimEvent;

    private FMOD.Studio.EventInstance walkInstance;
    public FMODUnity.EventReference walkEvent;

    private void Start()
    {
        cc = transform.gameObject.GetComponent<CharacterController>();
        swimInstance = FMODUnity.RuntimeManager.CreateInstance(swimEvent);
        walkInstance = FMODUnity.RuntimeManager.CreateInstance(walkEvent);
        RuntimeManager.AttachInstanceToGameObject(walkInstance, transform);
        walkInstance.start();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, floorMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButton("Jump") && !inSubmarine)
        {
            if(transform.position.y < waterSurface.position.y)
                velocity.y = Mathf.Sqrt(0.5f * -2 * Gravedad);
            else
                RuntimeManager.PlayOneShot("event:/Out_Water");
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * Velocidad * Time.deltaTime);

        if (inSubmarine && isGrounded && (x != 0 || z != 0))
            walkInstance.setParameterByName("Walk", 1);
        else
            walkInstance.setParameterByName("Walk", 0);

        velocity.y += Gravedad * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        swimInstance.setParameterByName("whales", lM.getTimeOfDay());
    }

    private void OnTriggerStay(Collider other)
    {
        Trigger tr = other.GetComponent<Trigger>();

        if (tr == null || tr.TriggerType != Trigger.trigger.trampilla)
            return;

        if (transform.position.y + transform.GetComponent<CharacterController>().height / 5 > other.transform.position.y)
        {
            if (!inSubmarine)
            {
                RuntimeManager.PlayOneShot("event:/Out_Water");
                swimInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                inSubmarine = true;
            }
        }
        else if (transform.position.y - transform.GetComponent<CharacterController>().height / 3 < other.transform.position.y)
        {
            if (inSubmarine)
            {
                RuntimeManager.PlayOneShot("event:/Jump_Water");
                swimInstance.start();
                inSubmarine = false;
            }
        }

    }

    private void OnDisable()
    {
        walkInstance.setParameterByName("Walk", 0);
    }
}
