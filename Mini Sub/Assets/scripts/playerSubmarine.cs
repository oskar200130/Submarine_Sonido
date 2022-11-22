using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSubmarine : MonoBehaviour
{
    private Transform selfTransform;
    public GameObject submarine;
    public Vector3 posSit, posUp;

    public bool driving = false;

    // Start is called before the first frame update
    void Start()
    {
        selfTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
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
    }
}
