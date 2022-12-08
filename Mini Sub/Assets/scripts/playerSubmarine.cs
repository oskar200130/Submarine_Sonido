using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSubmarine : MonoBehaviour
{
    public GameObject submarine, trapdoor;
    public Vector3 posSit, posUp;

    public bool driving = false;

    private Trigger trigger;

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
        if (trigger != null && Input.GetKeyDown(KeyCode.L))
        {
            switch (trigger.TriggerType)
            {
                case Trigger.trigger.volante:
                    changeSittin();
                    break;
                case Trigger.trigger.trampilla:
                    if (trapdoor.transform.rotation.z == 0)
                    {
                        trapdoor.transform.Rotate(new Vector3(0, 0, 1), -90);
                        trapdoor.GetComponentInChildren<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        trapdoor.transform.Rotate(new Vector3(0, 0, 1), 90);
                        trapdoor.GetComponentInChildren<BoxCollider>().enabled = true;
                    }
                    break;
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