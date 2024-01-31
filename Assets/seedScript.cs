using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedScript : MonoBehaviour
{
    private int seed;
    int setSeed(int seed)
    {
        UnityEngine.Random.InitState(seed);
        return seed;
    }
    int randomSeed()
    {
        seed = UnityEngine.Random.Range(0, 1000000000);
        return seed;
    }
    public int getSeed()
    {
        return seed;
    }
}
