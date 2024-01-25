using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class BacktrackData
{
    public Vector2 currentVect {get;set;}
    public string previousDirection {get;set; }
    public int previousRoomNumber {get;set;}
}

public class Backtrack : MonoBehaviour
{
    //creates custom array to store multiple types of data (max 50 rooms)
    BacktrackData[] dataArray = new BacktrackData[50];

    public int pointer = 1; //next available space

    void start()
    {
        dataArray[0] = new BacktrackData { currentVect = new Vector2(), previousDirection = "up", previousRoomNumber = -1 };
    }

    public void addBacktrack(Vector2 currentVect, string previousDirection, int previousRoomNumber)
    {
        dataArray[pointer] = new BacktrackData { currentVect = currentVect, previousDirection = previousDirection, previousRoomNumber = previousRoomNumber};
    }

    //function that returns values in array, based on the type of bakctrack (e.g. stuck or for maze bulding) and the random number of rooms it should bactrack
    public List<object> retireveInformation(string type, int randomNumber)
    {
        List<object> returnList = new List<object>();

        returnList.Add(dataArray[pointer-randomNumber].currentVect);
        returnList.Add(dataArray[pointer-randomNumber].previousDirection);
        returnList.Add(dataArray[pointer-randomNumber].previousRoomNumber);

        return returnList;
    }

    // IF ERRORS OCCUR AND NOTHING IN LOG THEN THE use of <object> above may be the reason. Mkae sure to cast objects back to their original type qhen retreieving info from this list.
}
