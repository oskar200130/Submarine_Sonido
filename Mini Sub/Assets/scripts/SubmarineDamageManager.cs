using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDamageManager : MonoBehaviour
{
    public BreakPoint[] breakingPoints;
    private bool[] isBroken;
    private float damage;
    private int numBreakingPoints;
    private int numBrokenPoints;
    [SerializeField] [Range(0f, 100f)] float totalDamage;
    public const int damageValue = 10;

    //Luces de alarma
    public GameObject[] lights;
    public Material lightMat;
    private bool alert = false;

    public bool isAlert() { return alert; }

    private FMOD.Studio.EventInstance instance;
    public EventReference fmodEvent;

    // Start is called before the first frame update
    void Start()
    {
        instance = RuntimeManager.CreateInstance(fmodEvent);
        RuntimeManager.AttachInstanceToGameObject(instance, lights[1].transform);
        instance.start();

        numBreakingPoints = breakingPoints.Length;
        isBroken = new bool[numBreakingPoints];
        damage = 0;
    }

    public void recieveDamage(float dmg)
    {
        damage += dmg;
        totalDamage += dmg;
        if (!alert)
        {
            alert = true;
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].GetComponent<Light>().color = Color.black;
            }
        }
        while (damage >= damageValue && numBrokenPoints < numBreakingPoints)
        {
            damage -= damageValue;
            int rnd = Random.Range(0, numBreakingPoints);

            while (isBroken[rnd])
            {
                rnd = (rnd + 1) % numBreakingPoints;
            }

            breakingPoints[rnd].gotBroken(totalDamage);
            isBroken[rnd] = true;
            numBrokenPoints++;
        }
      
    }

    public void breakingPointRepaired(GameObject go)
    {
        int id = 0;
        while(go != breakingPoints[id].transform.gameObject)
        {
            id++;
        }
        isBroken[id] = false;
        numBrokenPoints--;
        totalDamage -= damageValue;
    }

    public float getTotalDamg() { return totalDamage; }

    public void TurnOffAlarm()
    {
        if (!alert) return;
        
        alert = !alert;
        lightMat.SetColor("_EmissionColor", Color.white);
        lightMat.color = Color.white;
        instance.setParameterByName("Damage", 0);

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].GetComponent<Light>().color = Color.white;
            lights[i].GetComponent<Light>().intensity = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(alert)
        {
            Color lerpedColor = Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time, 1));
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].GetComponent<Light>().color = lerpedColor;
                lights[i].GetComponent<Light>().intensity = 10;
            }
            lightMat.color = lerpedColor;
            lightMat.SetColor("_EmissionColor", lerpedColor);
            
            instance.setParameterByName("Damage", totalDamage);
        }
    }

    private void OnDestroy()
    {
        instance.release();
        lightMat.SetColor("_EmissionColor", Color.white);
        lightMat.color = Color.white;
    }
}
