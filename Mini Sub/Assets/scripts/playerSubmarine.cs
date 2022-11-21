using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSubmarine : MonoBehaviour
{
    private Transform selfTransform;
    public subController submarine;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            driving = !driving;
            submarine.enabled = driving;
            this.gameObject.GetComponent<playerController>().enabled = !driving;
            this.gameObject.GetComponent<CharacterController>().enabled = !driving;
            if (driving)
                transform.localPosition = posSit;
            else
                transform.localPosition = posUp;
        }
    }
}
