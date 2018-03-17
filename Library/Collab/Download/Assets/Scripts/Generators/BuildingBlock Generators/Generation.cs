using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {

    public GameObject child;
    public GameObject extra;
    public float chanceForExtra;
    private float currentsize;
    public int maxComplexity;
    public bool creatingExtras = false;
    public float extraGrow = 0f;
    public bool hasExtra;

    public virtual void newChild(Vector3 inputLoc, int c)
    {
    }

    public virtual void extraStuff(float growthrate, float extraThreshold)
    {
    }

    public void createNewChance()
    {
        float randomFloat = Random.Range(0f, 1f);

        if (chanceForExtra >= randomFloat)
        {
            creatingExtras = true;
        }
    }
}
