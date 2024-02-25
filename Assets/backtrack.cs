using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//don't need anymore
using System.Text;
public class Backtrack : MonoBehaviour
{
    //creates custom array to store multiple types of data (max 50 rooms atm)
    Tuple<Vector2, string, int>[] dataArray = new Tuple<Vector2, string, int>[100];

    public int pointer = 1; //next available space

    void Start()
    {
        dataArray[0] = new Tuple<Vector2, string, int>(new Vector2(0,8), "up", -1);
    }

    public void addBacktrack(Vector2 currentVect, string previousDirection, int previousRoomNumber)
    {
        /* var tupleToCheck = dataArray[pointer - 1];
        if (tupleToCheck.Item1 == currentVect)
        {
            return;
        } */
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

    public void printAllTuples()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < pointer; i++)
        {
            var tuple = dataArray[i];
            sb.AppendLine($"Index: {i}, Vector2: {tuple.Item1}, Direction: {tuple.Item2}, Room Number: {tuple.Item3}");
        }
        Debug.Log(sb.ToString());
    }

    public void clearBacktrack()
    {
        for (int i = 0; i < pointer; i++)
        {
            dataArray[i] = null;
        }
        pointer = 0;
    }

    // EXIT STUFF

    Dictionary<int, GameObject> exitObjects = new Dictionary<int, GameObject>();

    public int generateKey(Vector2 currentPosition)
    {
        int key = ((int)currentPosition.x * 10000) + (int)currentPosition.y;
        return key;
    }

    public void addExitObject(Vector2 coordinates, GameObject exitObject)
    {
        int key = generateKey(coordinates);
        exitObjects[key] = exitObject;
    }

    public void removeExitObject(Vector2 coordinates)
    {
        
        int key = generateKey(coordinates);

        if (exitObjects.ContainsKey(key))
        {
            GameObject exit = exitObjects[key];
            Destroy(exit);
            Debug.Log("Exit removed at" + coordinates);
        } else {
            Debug.Log("Key not found");
            return;
        }
    }

    Dictionary<Vector2, int[]> directionsTriedDictionary = new Dictionary<Vector2, int[]>();

    public void writeDirectionToDictionary(Vector2 currentPosition, string direction)
    {
        if (directionsTriedDictionary.ContainsKey(currentPosition))
        {
            int[] directionsTried = directionsTriedDictionary[currentPosition];
            var directionNum = getDirNum(direction);

            directionsTried[directionNum] = 1;
        }
        else
        {
            int[] directionsTried = new int[5];
            directionsTried[getDirNum(direction)] = 1;
            directionsTriedDictionary[currentPosition] = directionsTried;
        }
    }

    public bool checkDirInDict(Vector2 currentPosition, string direction)
    {
        if (directionsTriedDictionary.ContainsKey(currentPosition))
        {
            int[] directionsTried = directionsTriedDictionary[currentPosition];
            if (directionsTried[getDirNum(direction)] == 1)
            {
                return true;
            }
        }
        return false;
    }

    public bool dirDictKeyExists(Vector2 currentPosition)
    {
        return directionsTriedDictionary.ContainsKey(currentPosition);
    }

    public int[] getDictArray(Vector2 currentVect)
    {
        return directionsTriedDictionary[currentVect];
    }

    int getDirNum(string direction)
    {
        int directionNum = -1;
        if (direction == "right")
        {
            directionNum = 0;
        }
        else if (direction == "left")
        {
            directionNum = 1;
        }
        else if (direction == "up")
        {
            directionNum = 2;
        }
        else if (direction == "down")
        {
            directionNum = 3;
        }
        return directionNum;
    }

    public void roomHasSpawned(Vector2 currentVect)
    {
        var array = directionsTriedDictionary[currentVect];
        array[4] = 1;
    }

    public void roomHasntSpawned(Vector2 currentVect)
    {
        var array = directionsTriedDictionary[currentVect];
        array[4] = 0;
    }

    public bool checkFirstIteration(Vector2 currentVect)
    {
        //if room exists then return true
        if (directionsTriedDictionary.ContainsKey(currentVect) == false)
        {
            return true;
        } else {
            var array = directionsTriedDictionary[currentVect];
            if (array[4] == 0)
            {
                return true;
            } else {
                return false;
            }
        }
    }

}