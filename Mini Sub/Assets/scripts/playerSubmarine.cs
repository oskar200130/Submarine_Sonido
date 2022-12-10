using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSubmarine : MonoBehaviour
{
    public GameObject submarine, trapdoor;
    public Vector3 posSit, posUp;

    public bool driving = false;

    private Trigger trigger;
    private bool trapdoorLerping = false;
    private int degrees = -1;
    private bool checkStay = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void changeSittin()
    {
        driving = !driving;
        transform.Rotate(new Vector3(0, 1, 0), 180);
        transform.parent.transform.Rotate(new Vector3(0, 1, 0), 180);
        submarine.GetComponent<subController>().enabled = driving;
        submarine.GetComponent<Rigidbody>().isKinematic = !driving;
        this.gameObject.GetComponent<playerController>().enabled = !driving;
        this.gameObject.GetComponent<CharacterController>().enabled = !driving;
        if (driving)
            transform.localPosition = posSit;
        else
            transform.localPosition = posUp;
    }

    private void Update()
    {
        if (trigger != null && Input.GetMouseButtonDown(0) && !submarine.GetComponent<SubmarineDamageManager>().isAlert())
        {
            switch (trigger.TriggerType)
            {
                case Trigger.trigger.volante:
                    changeSittin();
                    break;
                case Trigger.trigger.trampilla:
                    if (trapdoorLerping) break;
                    
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Trapdoor");
                    if (trapdoor.transform.localEulerAngles.z > -0.1 && trapdoor.transform.localEulerAngles.z < 0.1)                    
                        degrees = -90;                                        
                    else                    
                        degrees = 360;
                    
                    trapdoorLerping = true;
                    break;
            }
        }
        if (trigger != null && Input.GetMouseButtonDown(1))
        {
            if (trigger.TriggerType == Trigger.trigger.reparacion)
            {
                trigger.transform.GetComponentInParent<BreakPoint>().gotReapered();
                submarine.GetComponent<SubmarineDamageManager>().breakingPointRepaired(trigger.transform.parent.gameObject);
                checkStay = true;
                trigger = null;
            }
        }

        if (trapdoorLerping)
        {
            trapdoor.transform.localEulerAngles = Vector3.Lerp(trapdoor.transform.localEulerAngles, new Vector3(0, 0, degrees), Time.deltaTime);
            if (degrees != -1 && (trapdoor.transform.localEulerAngles.z < 270.5 || trapdoor.transform.localEulerAngles.z > 359.5))
            {
                trapdoor.GetComponentInChildren<BoxCollider>().enabled = !trapdoor.GetComponentInChildren<BoxCollider>().enabled;
                trapdoor.transform.localEulerAngles = new Vector3(0, 0, degrees);
                trapdoorLerping = false;
                degrees = -1;
            }
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Trigger>() != null)
        {
            trigger = other.GetComponent<Trigger>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (checkStay)
        {
            if (other.GetComponent<Trigger>() != null)
            {
                trigger = other.GetComponent<Trigger>();
            }
            checkStay = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Trigger>() != null)
        {
            trigger = null;
        }
    }
}