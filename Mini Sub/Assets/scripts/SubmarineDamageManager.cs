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

    // Start is called before the first frame update
    void Start()
    {
        numBreakingPoints = breakingPoints.Length;
        isBroken = new bool[numBreakingPoints];
        damage = 0;
    }

    void recieveDamage(float dmg)
    {
        damage += dmg;
        while(damage > 10 && numBrokenPoints < numBreakingPoints)
        {
            damage -= 10;
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

    void breakingPointRepaired(int id)
    {
        isBroken[id] = false;
        numBrokenPoints--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
