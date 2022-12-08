using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDamageManager : MonoBehaviour
{
    public GameObject[] breakingPoints;
    private bool[] isBroken;
    private float damage;
    private int numBreakingPoints;
    private int numBrokenPoints;
    [SerializeField][Range(0f, 100f)] float totalDamage;
    public const int damageValue = 10;

    //Luces de alarma
    public GameObject[] lights;
    public Material lightMat;
    private bool alert = false;

    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

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
        while(damage > damageValue && numBrokenPoints < numBreakingPoints)
        {
            damage -= damageValue;
            int rnd = Random.Range(0, numBreakingPoints);

            while (isBroken[rnd])
            {
                rnd = rnd + 1 % numBreakingPoints;
            }

            //breakinPoints.seRompen()
            isBroken[rnd] = true;
            numBrokenPoints++;
        }
      
    }

    public void breakingPointRepaired(int id)
    {
        isBroken[id] = false;
        numBrokenPoints--;
        totalDamage -= damageValue;
        if (!alert && totalDamage > 40)
        {
            alert = true;
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].GetComponent<Light>().color = Color.black;
            }
        }
    }

    public float getTotalDamg() { return totalDamage; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))        //QUITAR TODO IMPUT CON G(SOLO PARA DEBUG)
        {
            alert = !alert;
            totalDamage = 50;
        }
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
        }

        instance.setParameterByName("Damage", totalDamage);
        instance.setParameterByName("ActDamage", totalDamage);
    }

    private void OnDestroy()
    {
        instance.release();
        lightMat.SetColor("_EmissionColor", Color.white);
    }
}
