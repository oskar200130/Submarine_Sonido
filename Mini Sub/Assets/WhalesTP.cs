using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhalesTP : MonoBehaviour
{
    public Transform player;
    public float distance;

    // Start is called before the first frame update
    void OnEnable()
    {
        Vector3 pos = player.position;
        float difX = Random.Range(-distance, distance);
        pos.x += difX;
        pos.z += Random.Range(-(distance - difX), (distance - difX));
        this.transform.position = pos;
        GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
