using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedScript : MonoBehaviour
{
    private int seed;
    public int setSeed(int seedInp)
    {
        seed = seedInp;
        UnityEngine.Random.InitState(seedInp);
        return seed;
    }
    public int randomSeed()
    {
        seed = UnityEngine.Random.Range(0, 1000000000);
        return seed;
    }
    public int getSeed()
    {
        return seed;
    }
}
