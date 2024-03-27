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
        seed = UnityEngine.Random.Range(0, 2147483647); //32 bit integer limit
        UnityEngine.Random.InitState(seed);
        PlayerPrefs.SetInt("seed", seed);
        Debug.Log(seed);
        return seed;
    }
    public int getSeed()
    {
        return seed;
    }
}
