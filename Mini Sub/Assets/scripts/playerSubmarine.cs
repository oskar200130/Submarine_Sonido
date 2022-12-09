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
    private int degrees = 0;

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
        if (trigger != null && !trapdoorLerping && Input.GetKeyDown(KeyCode.L))
        {
            switch (trigger.TriggerType)
            {
                case Trigger.trigger.volante:
                    changeSittin();
                    break;
                case Trigger.trigger.trampilla:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Trapdoor");
                    if (trapdoor.transform.rotation.z == 0)                    
                        degrees = -90;                                        
                    else                    
                        degrees = 0;
                    
                    trapdoorLerping = true;
                    break;
            }
        }

        if (trapdoorLerping)
        {
            trapdoor.transform.rotation = Quaternion.Lerp(trapdoor.transform.rotation, Quaternion.AngleAxis(degrees, new Vector3(0, 0, 1)), Time.deltaTime*2);
            if(degrees == 0 && trapdoor.transform.rotation.eulerAngles.z > 359)
            {
                trapdoor.GetComponentInChildren<BoxCollider>().enabled = true;
                trapdoor.transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 0));
                trapdoorLerping = false;
            }
            if (degrees == -90 && trapdoor.transform.rotation.eulerAngles.z < 271)
            {
                trapdoor.GetComponentInChildren<BoxCollider>().enabled = false;
                trapdoor.transform.rotation = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
                trapdoorLerping = false;
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

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Trigger>() != null)
        {
            trigger = null;
        }
    }
}