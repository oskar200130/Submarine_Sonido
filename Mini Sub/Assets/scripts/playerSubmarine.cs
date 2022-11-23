using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSubmarine : MonoBehaviour
{
    private Transform selfTransform;
    public GameObject submarine;
    public Vector3 posSit, posUp;

    public bool driving = false;

    private Collider trigger;

    // Start is called before the first frame update
    void Start()
    {
        selfTransform = gameObject.GetComponent<Transform>();
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
        if (trigger != null && Input.GetKeyDown("q"))
        {
            changeSittin();
        }

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DriveSubmarine>() != null)
        {
            trigger = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DriveSubmarine>() != null)
        {
            trigger = null;
        }
    }
}
