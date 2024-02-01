using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Backtrack : MonoBehaviour
{
    //creates custom array to store multiple types of data (max 50 rooms atm)
    Tuple<Vector2, string, int>[] dataArray = new Tuple<Vector2, string, int>[50];

    public int pointer = 1; //next available space

    void Start()
    {
        dataArray[0] = new Tuple<Vector2, string, int>(new Vector2(0,8), "up", -1);
    }

    public void addBacktrack(Vector2 currentVect, string previousDirection, int previousRoomNumber)
    {
        dataArray[pointer] = new Tuple<Vector2, string, int>(currentVect, previousDirection, previousRoomNumber);
        pointer++;
    }

    public Tuple<Vector2, string, int> retrieveInformation(int randomNumber)
    {
        if (randomNumber == 0)
        {
            return dataArray[pointer - 1];
        }
        else
        {
            return dataArray[pointer - 1 - randomNumber];
        }
    }

    public void testing(int param)
    {
        Debug.Log("Succesffuly accessed backtrack with parameter " + param);
    }
}