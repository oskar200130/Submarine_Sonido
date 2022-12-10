using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gotBroken(float dmg)
    {
        transform.GetChild(0).GetComponent<FMODUnity.StudioEventEmitter>().EventInstance.setParameterByName("Damage", 1);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void gotReapered()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
